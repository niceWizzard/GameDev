using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Main.Lib.Mobs;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.Player;
using Main.UI;
using Main.World.Objects.CompletionStatue;
using Main.World.Objects.Door;
using Main.World.Objects.Pedestal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Level
{
    public enum LevelState
    {
        Playing,
        Died,
        Finished,
    }
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform safeSpawn;
        [Header("Completion Requirements")] 
        [SerializeField] private CompletionStatue completionStatue;
        public List<Requirement> _requirements = new();
        [SerializeField] private EnemyController boss;
        
        public List<Requirement> Requirements => _requirements;
        
        private List<int> _mobsInLevel = new(); 
        private List<int> _deadMobsInLevel = new();
        
        public List<int> MobsInLevel => _mobsInLevel;
        public List<int> DeadMobsInLevel => _deadMobsInLevel;

        public List<PedestalController> TotalPedestals { get; private set; } = new();
        public List<PedestalController> ActivatedPedestals => TotalPedestals.Where(v => v.IsActive).ToList();
        public List<string> CollectedKeys { get; private set; } = new();
        public int AliveMobs => _mobsInLevel.Count - DeadMobsInLevel.Count;

        private LevelState _state = LevelState.Playing;

        private void Awake()
        {
            if(safeSpawn == null)
                Debug.LogError($"Safe spawn is null at {name}");
            _requirements = GetComponentsInChildren<Requirement>().ToList();
            if (_requirements.Count > 0 )
            {
                if(!completionStatue)
                    Debug.LogError($"Completion statue is null at {name}");
                if(boss)
                    Debug.LogError($"There should be no requirements if there is a boss!");
            }
            
            var player = FindAnyObjectByType<PlayerController>();
            GameManager.Instance.RegisterLevelManager(this);
            player.HealthComponent.OnHealthZero += PlayerOnDie;
            player.Position = safeSpawn.position;
            
        }

        private void Start()
        {
            if (!boss) return;
            if(!completionStatue)
                Debug.LogError($"Completion Statue is required if there is a boss. at {name}");
            boss.HealthComponent.OnHealthZero += OnBossDie;
            HUDController.Instance.RegisterBoss(boss);
        }

        private void OnBossDie()
        {
            completionStatue.Setup();
        }

        private void PlayerOnDie()
        {
            _state = LevelState.Died;
            Invoke(nameof(PlayerDeathMenu), 0.5f);
        }

        public void PlayerDeathMenu()
        {
            GameManager.DiedAtLevel = SceneManager.GetActiveScene().name;
            LevelLoader.Instance.LoadHub();
        }

        private void OnDestroy()
        {
            GameManager.Instance.UnregisterLevelManager();
        }


        private void Update()
        {
            switch (_state)
            {
                case LevelState.Playing:
                    if (Input.GetKeyDown(KeyCode.Escape) && Mathf.Approximately(Time.timeScale, 1))
                        Dialog.CreateDialog("Pausing?", new List<(string, Action)>()
                        {
                            ("Continue", null),
                            ("Exit",  () =>
                            {
                                _ = DoExit();
                            })
                        });
                    if (_requirements.Count == 0 || !_requirements.All(v => v.CheckCompleted()))
                        return;
                    SpawnCompletionMenu();
                    SaveAsCompleted();
                    _state = LevelState.Finished;
                    break;
                case LevelState.Died:
                case LevelState.Finished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return;

            async UniTask DoExit()
            {
                await Dialog.CloseDialog();
                LevelLoader.Instance.LoadHub();
            }
        }

        private void SpawnCompletionMenu()
        {
            completionStatue.Setup();
        }

        private void SaveAsCompleted()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            _ = SaveManager.Instance.SaveDataAsync(v => v with
            {
                CompletedLevels = v.CompletedLevels.Append(sceneName).ToHashSet()
            });
        }

        public void RegisterAsDead(GameObject mob)
        {
            _deadMobsInLevel.Add(mob.GetInstanceID());
        }

        public void Register(MonoBehaviour script)
        {
            switch (script)
            {
                case PedestalController controller: 
                    TotalPedestals.Add(controller);
                    break;
                case MobController:
                    _mobsInLevel.Add(script.GetInstanceID());
                    break;
                case KeyItem key:
                    CollectedKeys.Add(key.UniqueId.Id);
                    break;
                default:
                    throw new ArgumentException($"Type {script.GetType()} is not supported");
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Transform completionStatueLocation;
        [Header("Completion Reward Choices")]
        [SerializeField]
        private StatType leftReward = StatType.Health;
        [SerializeField]
        private StatType rightReward = StatType.Speed;
        [Header("Completion Requirements")] [SerializeField]
        private List<Requirement> requirements = new();
        
        public List<Requirement> Requirements => requirements;
        
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
            if(!completionStatueLocation)
                Debug.LogWarning($"Completion statue location is null at {name}");
            var player = FindAnyObjectByType<PlayerController>();
            MainCamera.Instance.Follow(player);
            player.Position = safeSpawn.position;
            HUDController.Instance.SetPlayer(player);
            GameManager.Instance.RegisterLevelManager(this);
            player.HealthComponent.OnHealthZero += PlayerOnDie;
        }

        private void PlayerOnDie()
        {
            _state = LevelState.Died;
            MenuManager.Instance.ShowDeathMenu();
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
                    if (Input.GetKeyDown(KeyCode.Escape))
                        MenuManager.Instance.TogglePauseMenu();
                    if (requirements.Count == 0 || !requirements.All(v => v.CheckCompleted()))
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
            
        }

        private void SpawnCompletionMenu()
        {
            var statue = Instantiate(LevelLoader.Instance.CompletionStatuePrefab, (Vector2) completionStatueLocation.position, Quaternion.identity);
            statue.Setup(leftReward, rightReward);
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

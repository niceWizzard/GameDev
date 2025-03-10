using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.Player;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

        [Header("Completion Requirements")] [SerializeField]
        private List<Requirement> requirements = new();
        
        public List<Requirement> Requirements => requirements;
        
        private List<int> _mobsInLevel = new(); 
        private List<int> _deadMobsInLevel = new();
        
        public List<int> MobsInLevel => _mobsInLevel;
        public List<int> DeadMobsInLevel => _deadMobsInLevel;
        
        public int AliveMobs => _mobsInLevel.Count - DeadMobsInLevel.Count;

        private LevelState _state = LevelState.Playing;

        private void Awake()
        {
            if(safeSpawn == null)
                Debug.LogError($"Safe spawn is null at {SceneManager.GetActiveScene().name}");
            var player = FindAnyObjectByType<PlayerController>();
            MainCamera.Instance.Follow(player);
            player.transform.position = safeSpawn.position;
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
                    MenuManager.Instance.ShowCompletionMenu();
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

        private void SaveAsCompleted()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            var sceneId = LevelLoader.Instance.GetLevelGuid(sceneName);
            _ = SaveManager.Instance.SaveDataAsync(v => v with
            {
                CompletedLevels = v.CompletedLevels.Append(sceneId).ToList() 
            });
        }

        public void RegisterMob(GameObject mob)
        {
            _mobsInLevel.Add(mob.GetInstanceID());
        }

        public void RegisterAsDead(GameObject mob)
        {
            _deadMobsInLevel.Add(mob.GetInstanceID());
        }
    }
}

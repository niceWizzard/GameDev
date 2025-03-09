using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Lib.Singleton;
using Main.Player;
using Main.UI;
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

        [Header("Completion Requirements")] [SerializeField]
        private bool wipeoutEnemies = false;
        
        private List<Requirement> _requirements = new();
        
        public List<Requirement> Requirements => _requirements;
        
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
            player.transform.position = safeSpawn.position;
            MainCamera.Instance.Follow(player);
            HUDController.Instance.SetPlayer(player);
            if(wipeoutEnemies)
                _requirements.Add(new WipeoutEnemies());
            GameManager.Instance.RegisterLevelManager(this);

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
                    if (!_requirements.All(v => v.CheckCompleted()))
                        return;
                    _state = LevelState.Finished;
                    break;
                case LevelState.Died:
                    break;
                case LevelState.Finished:
                    Time.timeScale = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
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

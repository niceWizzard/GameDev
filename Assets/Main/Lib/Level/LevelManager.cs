using System;
using System.Collections;
using System.Collections.Generic;
using Main.Lib.Singleton;
using Main.Player;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform safeSpawn;
        
        
        private List<int> _mobsInLevel = new(); 
        private List<int> _deadMobsInLevel = new();
        
        public List<int> MobsInLevel => _mobsInLevel;
        public List<int> DeadMobsInLevel => _deadMobsInLevel;
        
        public int AliveMobs => _mobsInLevel.Count - DeadMobsInLevel.Count;

        private void Awake()
        {
            if(safeSpawn == null)
                Debug.LogError($"Safe spawn is null at {SceneManager.GetActiveScene().name}");
            var player = FindAnyObjectByType<PlayerController>();
            MainCamera.Instance.Follow(player);
            HUDController.Instance.SetPlayer(player);
            GameManager.Instance.RegisterLevelManager(this);
            StartCoroutine(SetPlayerPosition(player));
        }

        private void OnDestroy()
        {
            GameManager.Instance.UnregisterLevelManager();            
        }

        private  IEnumerator SetPlayerPosition(PlayerController player)
        {
            yield return new WaitForEndOfFrame();
            player.transform.position = safeSpawn.position;
            MainCamera.Instance.MoveTo(safeSpawn.position);
        }

        private void Update()
        {
            var aliveMobs  = _mobsInLevel.Count - _deadMobsInLevel.Count;
            if (aliveMobs > 0)
                return;
            Debug.Log("Completed");
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

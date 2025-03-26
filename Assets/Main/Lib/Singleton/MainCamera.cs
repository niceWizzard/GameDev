using System;
using System.Linq;
using Main.Lib.Level;
using Main.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;

namespace Main.Lib.Singleton
{
    public class MainCamera : PrefabSingleton<MainCamera>
    {
        [SerializeField] private CinemachineCamera cmCamera;
        [SerializeField] private Camera mainCamera;
        public Camera Camera => mainCamera;

        private void Start()
        {
            GameManager.LevelLoaded += GameManagerOnLevelLoaded;
        }

        private void OnDestroy()
        {
            GameManager.LevelLoaded -= GameManagerOnLevelLoaded;
        }

        private void GameManagerOnLevelLoaded(LevelManager level)
        {
            CinemachineImpulseManager.Instance.Clear();
        }

        public void Follow(PlayerController playerController)
        {
            cmCamera.Target.TrackingTarget = playerController.transform;   
            cmCamera.ForceCameraPosition(playerController.transform.position, Quaternion.identity);
        }

    }
}

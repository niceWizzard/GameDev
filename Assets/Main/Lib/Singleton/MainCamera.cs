using Main.Lib.Level;
using Main.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Main.Lib.Singleton
{
    public class MainCamera : PrefabSingleton<MainCamera>
    {
        [SerializeField] private CinemachineCamera cmCamera;
        [SerializeField] private Camera mainCamera;
        public Camera Camera => mainCamera;

        private void Start()
        {
            GameManager.LevelLoaded += OnLevelLoad;
            GameManager.LevelUnload += OnLevelUnload;
        }

        

        private void OnDestroy()
        {
            GameManager.LevelLoaded -= OnLevelLoad;
            GameManager.LevelUnload -= OnLevelUnload;
        }

        private void OnLevelLoad(LevelManager level)
        {
            CinemachineImpulseManager.Instance.Clear();
        }

        private void OnLevelUnload()
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

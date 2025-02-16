using System.Linq;
using Main.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main.Lib.Singleton
{
    public class MainCamera : Singleton<MainCamera>
    {
        [SerializeField] private CinemachineCamera cmCamera;
        [SerializeField] private Camera mainCamera;

        public Camera Camera => mainCamera;
        public void Follow(PlayerController playerController)
        {
            cmCamera.Target.TrackingTarget = playerController.transform;            
        }

        public static void TryInitializePrefab()
        {
            if (IsInitialized)
                return;
            var a = Addressables.LoadAssetsAsync<GameObject>("MainCamera").WaitForCompletion().First();
            Instantiate(a);
        }

        public void MoveTo(Vector2 newPos)
        {
            cmCamera.ForceCameraPosition(newPos, Quaternion.identity);
        }
    }
}

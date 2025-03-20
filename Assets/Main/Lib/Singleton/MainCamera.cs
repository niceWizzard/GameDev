using System.Linq;
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
        public void Follow(PlayerController playerController)
        {
            cmCamera.Target.TrackingTarget = playerController.transform;   
            cmCamera.ForceCameraPosition(playerController.transform.position, Quaternion.identity);
        }

    }
}

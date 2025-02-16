using Main.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Main.Lib.Singleton
{
    public class MainCamera : Singleton<MainCamera>
    {
        [SerializeField] private CinemachineCamera cmCamera;
        public void Follow(PlayerController playerController)
        {
            cmCamera.Target.TrackingTarget = playerController.transform;            
        }
    }
}

using System.Collections.Generic;
using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main.Lib.Level
{
    [RequireComponent(typeof(BoxCollider2D)), ExecuteAlways]
    public class LevelSwitcher : MonoBehaviour
    {
        private static readonly Dictionary<UniqueIdentifier, LevelSwitcher> DoorToLevelSwitchers = new();
        public static LevelSwitcher FindLevelSwitch(UniqueIdentifier uniqueIdentifier)
        {
            return DoorToLevelSwitchers[uniqueIdentifier];
        }
        [SerializeField] private AssetReferenceT<UniqueIdentifier> doorIdentifier = null!;
        [SerializeField] private AssetReferenceT<UniqueIdentifier> targetDoorIdentifier = null!;
        [SerializeField] private Vector2 safePosition;
        
        public UniqueIdentifier UniqueIdentifier { get; private set; }
        public UniqueIdentifier TargetUniqueIdentifier { get; private set; }
        
        public Vector2 SafePosition => (Vector2)transform.position + safePosition;

        private void Awake()
        {
            if (!Application.isPlaying)
                return;
            if(doorIdentifier == null)
                Debug.LogError("DoorIdentifier is null!");
            if(targetDoorIdentifier == null)
                Debug.LogError("TargetDoorIdentifier is null!");
            UniqueIdentifier = doorIdentifier.LoadAssetAsync().WaitForCompletion();
            TargetUniqueIdentifier = targetDoorIdentifier.LoadAssetAsync().WaitForCompletion();
            DoorToLevelSwitchers.Add(UniqueIdentifier, this);
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying)
                return;
            DoorToLevelSwitchers.Remove(UniqueIdentifier);
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            Gizmos.DrawWireSphere(transform.position + (Vector3)safePosition, 0.1f);
            #endif
        }

        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Application.isPlaying)
                return;
            if (!other.TryGetComponent<PlayerController>(out var player))
                return;
            LevelLoader.Instance.GoToLevel(TargetUniqueIdentifier);
        }

        
    }
}

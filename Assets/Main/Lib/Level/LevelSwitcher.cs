using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main.Lib.Level
{
    [RequireComponent(typeof(BoxCollider2D), typeof(UniqueId)), ExecuteAlways]
    public class LevelSwitcher : MonoBehaviour
    {
        private static readonly Dictionary<UniqueId, LevelSwitcher> DoorToLevelSwitchers = new();
        public static LevelSwitcher FindLevelSwitch(string uniqueIdentifier)
        {
            return DoorToLevelSwitchers.FirstOrDefault((a) =>a.Key.Id == uniqueIdentifier ).Value;
        }
        [SerializeField] private UniqueId doorIdentifier = null!;
        [SerializeField] private string targetDoorIdentifier = null!;
        [SerializeField] private Vector2 safePosition;
        
        public Vector2 SafePosition => (Vector2)transform.position + safePosition;

        private void Awake()
        {
            if (!Application.isPlaying)
                return;
            if(!doorIdentifier)
                Debug.LogError($"DoorIdentifier of {name} is null!");
            if(targetDoorIdentifier == null)
                Debug.LogError("TargetDoorIdentifier is null!");
            DoorToLevelSwitchers.Add(doorIdentifier, this);
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying)
                return;
            DoorToLevelSwitchers.Remove(doorIdentifier);
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
            LevelLoader.Instance.GoToLevel(targetDoorIdentifier);
        }

        
    }
}

using System;
using Lib;
using Player;
using UnityEngine;

namespace Mobs.Ghost
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GhostController : MobController
    {
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private Collider2D collider2d;
        [SerializeField] private MobDetector mobDetector;
    
        public Rigidbody2D Rigidbody2D => rigidbody2d;
        public Collider2D Collider2D => collider2d;

        [HideInInspector]
        public PlayerController detectedPlayer;

        protected override void Awake()
        {
            base.Awake();
            mobDetector.OnMobEntered += controller => detectedPlayer = controller as PlayerController;
            mobDetector.OnMobExited += controller => detectedPlayer = null;
        }

        private void Update()
        {
            Debug.Log(detectedPlayer == null ? "NO PLAYER" : "PLAYER DETECED: " + detectedPlayer.name);
        }
    }
}

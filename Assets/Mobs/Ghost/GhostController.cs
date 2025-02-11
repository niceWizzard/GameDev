using System;
using Lib;
using Lib.StateMachine;
using Mobs.Ghost.States;
using Player;
using UnityEngine;
using Weapons;

namespace Mobs.Ghost
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GhostController : MobController
    {
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private Collider2D collider2d;
        [SerializeField] private MobDetector mobDetector;
        [SerializeField] private GhostStateMachine ghostStateMachine;

        [SerializeField] private ProjectileController projectilePrefab;
        
        [Header("Stats")]
        [SerializeField] private float movementSpeed = 4.5f;
        
        public float MovementSpeed => movementSpeed;
    
        public Rigidbody2D Rigidbody2D => rigidbody2d;
        public Collider2D Collider2D => collider2d;

        [HideInInspector]
        public PlayerController detectedPlayer;
        
        public ProjectileController ProjectilePrefab => projectilePrefab;

        public bool CanAttack { get; set; } = true;

        protected  void Start()
        {
            mobDetector.OnMobEntered += controller =>
            {
                detectedPlayer = controller as PlayerController;
                ghostStateMachine.ChangeState(GhostState.HasTarget);
            };
        }
    }
}

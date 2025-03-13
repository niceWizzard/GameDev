using System;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using Main.Lib;
using Main.Player;
using Main.Weapons;
using Main.World.Mobs.Ghost.States;
using Unity.Mathematics;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GhostController : MobController
    {
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private Collider2D collider2d;
        [SerializeField] private MobDetector mobDetector;

        [SerializeField] private ProjectileController projectilePrefab;
        
        [Header("Stats")]
        [SerializeField] private float movementSpeed = 4.5f;
        [SerializeField] public LayerMask dangerMask;

        
        public float MovementSpeed => movementSpeed;
    
        public Rigidbody2D Rigidbody2D => rigidbody2d;
        public Collider2D Collider2D => collider2d;

        [HideInInspector]
        public PlayerController detectedPlayer;

        private UniqueId _uniqueId = null!;

        public ProjectileController ProjectilePrefab => projectilePrefab;

        public bool CanAttack { get; set; } = true;

        protected override void Awake()
        {
            base.Awake();
            _uniqueId = GetComponent<UniqueId>();
            GameManager.Instance.CurrentLevelManager.RegisterMob(gameObject);
        }

        protected override void OnHealthZero()
        {
            base.OnHealthZero();
            GameManager.Instance.CurrentLevelManager.RegisterAsDead(gameObject);
        }

        protected  void Start()
        {
            mobDetector.OnMobEntered += controller =>
            {
                detectedPlayer = controller as PlayerController;
            };
        }
        
        public Vector2 ContextBasedSteer(Vector2 desiredVelocity)
        {
            const int RAY_COUNT = 8;
            const float RAY_LENGTH = 1.5f;
            var rays = Enumerable.Range(0,RAY_COUNT).Select(i =>
            {
                var angle = (i * 2 * Mathf.PI / RAY_COUNT) * math.TODEGREES;
                Vector2 vec = Quaternion.Euler(0, 0, angle) * desiredVelocity;
                return vec.normalized;
            }).ToList();
            var interests = Enumerable.Range(0, RAY_COUNT).Select(i =>
            {
                var dot = Vector2.Dot(desiredVelocity.normalized, rays[i]);
                return dot;
            }).ToList();
            Collider2D.enabled = false;
            for (var i = 0; i < rays.Count; i++)
            {
                var ray = rays[i];
                var hit = Physics2D.Raycast(transform.position, ray, RAY_LENGTH, dangerMask);
                if (hit )
                {
                    interests[i] -= 3f;
                }
            }
            Collider2D.enabled = true;

            return rays.Select((v, i) => v * interests[i]).Aggregate(Vector2.zero, (current, next) => current + next).normalized;
        }
    }
}

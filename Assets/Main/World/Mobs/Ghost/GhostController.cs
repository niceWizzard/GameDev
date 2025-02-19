using CleverCrow.Fluid.UniqueIds;
using Main.Lib;
using Main.Player;
using Main.Weapons;
using Main.World.Mobs.Ghost.States;
using UnityEngine;

namespace Main.World.Mobs.Ghost
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

        private UniqueId _uniqueId = null!;

        public ProjectileController ProjectilePrefab => projectilePrefab;

        public bool CanAttack { get; set; } = true;

        protected override void Awake()
        {
            base.Awake();
            _uniqueId = GetComponent<UniqueId>();
        }

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

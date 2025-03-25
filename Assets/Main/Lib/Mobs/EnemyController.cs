using System.Linq;
using Main.Lib.Items;
using Main.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Main.Lib.Mobs
{
    /// <summary>
    /// This is the base class for all Enemies in the Game.
    /// All enemies must extend from this so they'll be registered to LevelManager
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public  abstract class EnemyController : MobController
    {
        [SerializeField] private MobDetector mobDetector;
        [Header("Stats")]
        [SerializeField] public LayerMask dangerMask;

        public NavMeshAgent NavMeshAgent { get; private set; }
        public MobDetector MobDetector => mobDetector;
        [HideInInspector]
        public PlayerController detectedPlayer;
        
        public ItemDropper MobItemDropper { get; set; }

        protected override void OnHealthZero()
        {
            base.OnHealthZero();
            GameManager.Instance.CurrentLevelManager.RegisterAsDead(gameObject);
            MobItemDropper.DropItems();
        }
        
        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.CurrentLevelManager.Register(this);
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updateUpAxis = false;
        }

        protected override void GetRequiredComponents()
        {
            base.GetRequiredComponents();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            MobItemDropper = GetComponent<ItemDropper>();
        }


        protected override void VerifyRequiredComponents()
        {
            base.VerifyRequiredComponents();
            if(!MobDetector)
                Debug.LogError($"Mob detector is missing on {name}. ");
            if(!NavMeshAgent)
                Debug.LogError($"NavMeshAgent is missing on {name}. ");
            if(!MobItemDropper)
                Debug.LogError($"MobItemDropper is missing on {name}. ");
        }


        protected  virtual void Start()
        {
            MobDetector.OnMobEntered += controller =>
            {
                detectedPlayer = controller as PlayerController;
            };
        }
        
        protected virtual void FixedUpdate()
        {
            NavMeshAgent.nextPosition = Position;
        }
        
        
        /// <summary>
        /// Supposed to let mob's movement detect obstacles and move accordingly.
        /// </summary>
        /// <param name="desiredVelocity">The direction the mob wants to go.</param>
        /// <returns>The steered velocity that is safe from obstacles</returns>
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
            Collider2d.enabled = false;
            for (var i = 0; i < rays.Count; i++)
            {
                var ray = rays[i];
                var hit = Physics2D.Raycast(Position, ray, RAY_LENGTH, dangerMask);
                if (hit )
                {
                    interests[i] -= 3f;
                }
            }
            Collider2d.enabled = true;

            return rays.Select((v, i) => v * interests[i]).Aggregate(Vector2.zero, (current, next) => current + next).normalized;
        }
    }
}

using System.Linq;
using Main.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Main.Lib.Mobs
{
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

        protected override void OnHealthZero()
        {
            base.OnHealthZero();
            GameManager.Instance.CurrentLevelManager.RegisterAsDead(gameObject);
        }
        
        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.CurrentLevelManager.RegisterMob(gameObject);
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updateUpAxis = false;
        }

        protected override void GetRequiredComponents()
        {
            base.GetRequiredComponents();
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected override void VerifyRequiredComponents()
        {
            base.VerifyRequiredComponents();
            if(!MobDetector)
                Debug.LogError($"Mob detector is missing on {name}. ");
            if(!NavMeshAgent)
                Debug.LogError($"NavMeshAgent is missing on {name}. ");
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

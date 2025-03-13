using System.Linq;
using Main.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Main.Lib.Mobs
{
    public  abstract class EnemyController : MobController
    {
        
        [SerializeField] protected MobDetector mobDetector;
        [SerializeField] protected NavMeshAgent navMeshAgent; 

        [Header("Stats")]
        [SerializeField] public LayerMask dangerMask;

        public NavMeshAgent NavMeshAgent => navMeshAgent;
        
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

        
        protected  virtual void Start()
        {
            mobDetector.OnMobEntered += controller =>
            {
                detectedPlayer = controller as PlayerController;
            };
        }
        
        protected virtual void FixedUpdate()
        {
            NavMeshAgent.nextPosition = transform.position;
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

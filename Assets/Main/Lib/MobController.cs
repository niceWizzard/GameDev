using Main.Lib.Health;
using Main.UI;
using UnityEngine;

namespace Main.Lib
{
    [RequireComponent(typeof(HealthComponent), typeof(Hurtbox), typeof(SpriteRenderer))]
    [RequireComponent( typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class MobController : MonoBehaviour
    {
        public HealthComponent HealthComponent { get; private set; }
        public Hurtbox Hurtbox { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody2d { get; private set; }
        public Collider2D Collider2d { get; private set; }
        
        [Header("Stats")]
        [SerializeField] protected float movementSpeed = 4.5f;

        public float MovementSpeed => movementSpeed;
    
    
        protected virtual void Awake()
        {
            GetRequiredComponents();
            VerifyRequiredComponents();
            Hurtbox.OnHurt += OnHurtboxHurt;
            HealthComponent.OnHealthZero += OnHealthZero;
        }

        protected virtual void GetRequiredComponents()
        {
            Rigidbody2d = GetComponent<Rigidbody2D>();
            Collider2d = GetComponent<Collider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Hurtbox = GetComponent<Hurtbox>();
            HealthComponent = GetComponent<HealthComponent>();
        }

        protected virtual void VerifyRequiredComponents()
        {
            if(!HealthComponent)
                Debug.LogError($"No health component attached to {name}");
            if(!Hurtbox)
                Debug.LogError($"No hurtbox attached to {name}");
            if(!SpriteRenderer)
                Debug.LogError($"No sprite renderer attached to {name}");
            if(!Rigidbody2d)
                Debug.LogError($"No rigidbody2d attached to {name}");
            if(!Collider2d)
                Debug.LogError($"No collider attached to {name}");
            
        }

        protected virtual void OnHurtboxHurt(DamageInfo damageInfo)
        {
            HealthComponent.ReduceHealth(damageInfo.damage);
        }

        protected virtual void OnHealthZero()
        {
            Destroy(gameObject);
        }
    
    }
}

using Main.Lib.Health;
using UnityEngine;

namespace Main.Lib
{
    public abstract class MobController : MonoBehaviour
    {
        [Header("Mob Components")]
        [SerializeField] protected HealthComponent healthComponent;
        [SerializeField] protected Hurtbox hurtbox;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Rigidbody2D rigidbody2d;
        [SerializeField] protected Collider2D collider2d;
        
        [Header("Stats")]
        [SerializeField] protected float movementSpeed = 4.5f;

        public float MovementSpeed => movementSpeed;
    
        public Rigidbody2D Rigidbody2D => rigidbody2d;
        public Collider2D Collider2D => collider2d;
    
        public HealthComponent HealthComponent => healthComponent;
        public Hurtbox Hurtbox => hurtbox;
        public SpriteRenderer SpriteRenderer => spriteRenderer;


        protected virtual void Awake()
        {
            hurtbox.OnHurt += OnHurtboxHurt;
            healthComponent.OnHealthZero += OnHealthZero;
        }

        protected virtual void OnHurtboxHurt(DamageInfo damageInfo)
        {
            healthComponent.ReduceHealth(damageInfo.damage);
        }

        protected virtual void OnHealthZero()
        {
            Destroy(gameObject);
        }
    
    }
}

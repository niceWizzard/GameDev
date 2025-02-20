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

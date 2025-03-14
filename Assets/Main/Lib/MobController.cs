using System;
using Main.Lib.Health;
using Main.Lib.Stat;
using Main.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.Lib
{
    /// <summary>
    /// This is the base class for all characters in the game.
    /// If you want to create a moving character, you can extend this class to reduce boilerplate
    /// </summary>
    [RequireComponent(typeof(HealthComponent), typeof(SpriteRenderer))]
    [RequireComponent( typeof(Rigidbody2D), typeof(Collider2D), typeof(Stats))]
    public abstract class MobController : MonoBehaviour
    {
        public HealthComponent HealthComponent { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody2d { get; private set; }
        public Collider2D Collider2d { get; private set; }
        
        public Stats Stats { get; private set; }
        
        /// <summary>
        /// The hurtbox of this Mob.
        /// Recommended to make another object with another collider as a hurtbox
        /// </summary>
        [SerializeField]
        private Hurtbox hurtbox;
        
        public Hurtbox Hurtbox => hurtbox;

        /// <summary>
        /// Shorthand for <see cref="Transform"/>.position
        /// </summary>
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        /// <summary>
        /// Shorthand for <see cref="Rigidbody2d"/>.linearVelocity
        /// </summary>
        public Vector2 Velocity
        {
            get => Rigidbody2d.linearVelocity;
            set => Rigidbody2d.linearVelocity = value;
        }

        public float MovementSpeed => Stats.MovementSpeed;
    
    
        protected virtual void Awake()
        {
            GetRequiredComponents();
            VerifyRequiredComponents();
            Hurtbox.OnHurt += OnHurtboxHurt;
            HealthComponent.OnHealthZero += OnHealthZero;
            HealthComponent.SetHealth(Stats.Health);
        }

        protected virtual void GetRequiredComponents()
        {
            Rigidbody2d = GetComponent<Rigidbody2D>();
            Collider2d = GetComponent<Collider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            HealthComponent = GetComponent<HealthComponent>();
            Stats = GetComponent<Stats>();
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
            if(!Stats)
                Debug.LogError($"No stats attached to {name}");
            
        }

        /// <summary>
        /// Gets called whenever the <see cref="Hurtbox"/> reports damage
        /// </summary>
        /// <param name="damageInfo"></param>
        protected virtual void OnHurtboxHurt(DamageInfo damageInfo)
        {
            HealthComponent.ReduceHealth(damageInfo.damage);
        }

        /// <summary>
        /// Gets called when the mob's health gets to zero.
        /// Calls Destroy by default.
        /// </summary>
        protected virtual void OnHealthZero()
        {
            Destroy(gameObject);
        }
    
    }
}

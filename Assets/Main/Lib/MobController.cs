using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Lib.Health;
using Main.Lib.Singleton;
using Main.Lib.Stat;
using Main.UI;
using Main.Weapons;
using Main.World.Mobs.Death_Animation;
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
    [RequireComponent( typeof(Animator))]
    public abstract class MobController : MonoBehaviour, IProjectileSender
    {
        public HealthComponent HealthComponent { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody2d { get; private set; }
        public Collider2D Collider2d { get; private set; }
        
        public Animator Animator { get; private set; }
        
        public Stats Stats { get; private set; }
        public AudioSource HurtAudioSource => hurtAudioSource;
        
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

        private bool _inHurtAnimation = false;

        [SerializeField] private AudioSource hurtAudioSource;
    
        protected virtual void Awake()
        {
            GetRequiredComponents();
            VerifyRequiredComponents();
            Hurtbox.OnHurt += OnHurtboxHurt;
            HealthComponent.OnHealthZero += OnHealthZero;
            HealthComponent.SetMaxHealth(Stats.Health);
        }

        protected virtual void GetRequiredComponents()
        {
            Rigidbody2d = GetComponent<Rigidbody2D>();
            Collider2d = GetComponent<Collider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            HealthComponent = GetComponent<HealthComponent>();
            Stats = GetComponent<Stats>();
            Animator = GetComponent<Animator>();
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
            if(!Animator)
                Debug.LogError($"No animator attached to {name}");
            
        }

        /// <summary>
        /// Gets called whenever the <see cref="Hurtbox"/> reports damage
        /// </summary>
        /// <param name="damageInfo"></param>
        protected virtual void OnHurtboxHurt(DamageInfo damageInfo)
        {
            _ = HurtAnimation();
            HealthComponent.ReduceHealth(damageInfo.damage);
            HurtAudioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
            HurtAudioSource.Play();
        }

        protected virtual async UniTask HurtAnimation()
        {
            if (_inHurtAnimation)
                return;
            if (this == null || !gameObject.activeInHierarchy)
                return;
            _inHurtAnimation = true;
            var origColor = SpriteRenderer.color;
            var a = SpriteRenderer.DOColor(Color.red, 0.1f).SetEase(Ease.InCubic).SetLink(gameObject);
            await a.AsyncWaitForCompletion();
            if (!this || !gameObject.activeInHierarchy)
                return;
            a = SpriteRenderer.DOColor(origColor, 0.1f).SetEase(Ease.InCubic).SetLink(gameObject);
            await a.AsyncWaitForCompletion();
            if (!this || !gameObject.activeInHierarchy)
                return;
            _inHurtAnimation = false;
        }

        /// <summary>
        /// Gets called when the mob's health gets to zero.
        /// Calls Destroy by default.
        /// </summary>
        protected virtual void OnHealthZero()
        {
            SenderDispose?.Invoke();
            SpawnDeathAnimation();
            Destroy(gameObject);
        }

        private void SpawnDeathAnimation()
        {
            var instance = PrefabLoader.SpawnDeathAnimation();
            instance.transform.position = Position;
            instance.transform.localScale = transform.localScale;
            instance.SpriteRenderer.flipX = SpriteRenderer.flipX;
            instance.SpriteRenderer.color = SpriteRenderer.color;
            instance.SpriteRenderer.spriteSortPoint = SpriteRenderer.spriteSortPoint;
            instance.SpriteRenderer.sortingOrder = SpriteRenderer.sortingOrder;
            instance.SpriteRenderer.sortingLayerID = SpriteRenderer.sortingLayerID;
            OnDeathAnimation(instance);
        }

        protected abstract void OnDeathAnimation(DeathAnimation deathAnimation);

        public GameObject GameObject => gameObject;
        public event Action SenderDispose;

    }
}

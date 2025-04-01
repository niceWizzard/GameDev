using System;
using Main.Lib.Health;
using Main.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Objects.Traps
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class TrapController : MonoBehaviour
    {
        private static readonly int IsActivated = Animator.StringToHash("is_activated");
        private Animator _animator;
        
        private bool _isActivated;
        [SerializeField] private float damage = 100f;
        [SerializeField]
        private Hitbox hitbox;

        private Collider2D _collider;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            hitbox.HurtboxHit += OnHurtboxHit;
            
        }

        private void Start()
        {
            hitbox.Disable();
            _collider.includeLayers = LayerMask.GetMask("Player Feet");
            hitbox.Collider.includeLayers = LayerMask.GetMask("Player Feet");
            hitbox.Collider.excludeLayers = LayerMask.GetMask("Player Hurtbox");
        }

        private void OnDestroy()
        {
            hitbox.HurtboxHit -= OnHurtboxHit;
        }

        private void OnHurtboxHit(Hurtbox hurtbox)
        {
            hurtbox.TakeDamage(new DamageInfo(damage, gameObject));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("PlayerFeet") || !other.attachedRigidbody || _isActivated)
                return;
            _isActivated = true;
            _animator.SetBool(IsActivated, true);
        }

        public void ActivationAnimationFinished()
        {
            _animator.SetBool(IsActivated, false);
        }

        public void DeactivationAnimationStart()
        {
            _isActivated = false;
            _animator.SetBool(IsActivated, false);
            hitbox.Disable();
        }

        public void EnableHitbox()
        {
            hitbox.Enable();
        }

    }
}

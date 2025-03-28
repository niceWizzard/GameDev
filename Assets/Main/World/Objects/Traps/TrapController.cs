using System;
using Main.Lib.Health;
using Main.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Objects.Traps
{
    public class TrapController : MonoBehaviour
    {
        private static readonly int IsActivated = Animator.StringToHash("is_activated");
        private Animator _animator;
        
        private bool _isActivated;
        [SerializeField] private float damage = 100f;
        [SerializeField]
        private Hitbox hitbox;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            hitbox.HurtboxHit += OnHurtboxHit;
        }

        private void Start()
        {
            hitbox.Disable();
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
            if (!other.CompareTag("Player") || !other.attachedRigidbody || _isActivated)
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
            hitbox.Disable();
        }

        public void EnableHitbox()
        {
            hitbox.Enable();
        }

    }
}

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
        [SerializeField]
        private Hitbox hitbox;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            hitbox.HurtboxHit += OnHurtboxHit;
            hitbox.Disable();
        }

        private void OnDestroy()
        {
            hitbox.HurtboxHit -= OnHurtboxHit;
        }

        private void OnHurtboxHit(Hurtbox hurtbox)
        {
            hurtbox.TakeDamage(new DamageInfo(30, gameObject));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _isActivated)
                return;
            Debug.Log("PLAYER ENTER");
            _isActivated = true;
            _animator.SetBool(IsActivated, true);
        }

        public void ActivationAnimationFinished()
        {
            Debug.Log("Animation end");
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

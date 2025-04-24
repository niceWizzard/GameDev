using System;
using Main.Lib.Health;
using UnityEngine;

namespace Main.World.Objects.Traps.Timed.TimedSpikes
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(AudioSource))]
    public class TimedTrapController : MonoBehaviour
    {
        private static readonly int IsActive = Animator.StringToHash("isActive");
        [SerializeField] private float damage = 101f;
        [SerializeField] private float startDelay = 0;
        [SerializeField] private float endDelay = 1.5f;
        [SerializeField] private Hitbox hitbox;
        private bool _isActivated = false;

        private float _time = 0;
        private Animator _animator;
        private AudioSource _audioSource;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            hitbox.Disable();
            hitbox.HurtboxHit += HitboxOnHurtboxHit;
            hitbox.Collider.includeLayers = LayerMask.GetMask("Player Feet");
            hitbox.Collider.excludeLayers = LayerMask.GetMask("Player Hurtbox");
            _time = startDelay;
            _audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        }
        
        public void PlayAudio()
        {
            _audioSource.Play();
        }

        private void OnDestroy()
        {
            hitbox.HurtboxHit -= HitboxOnHurtboxHit;
        }

        private void HitboxOnHurtboxHit(Hurtbox hurtbox)
        {
            hurtbox.TakeDamage(new DamageInfo(damage, gameObject));
        }

        private void Reset()
        {
            _time = endDelay;
            _isActivated = false;
            _animator.SetBool(IsActive, false);
            hitbox.Disable();
        }

        private void Update()
        {
            if (_isActivated)
                return;
            _time -= Time.deltaTime;
            if (_time > 0)
                return;
            _isActivated = true;
            hitbox.Enable();
            _animator.SetBool(IsActive, true);
        }
    }
}

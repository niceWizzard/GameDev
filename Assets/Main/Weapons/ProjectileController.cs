#nullable enable
using System;
using Main.Lib.Health;
using Main.Weapons.Gun;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Weapons
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class ProjectileController : MonoBehaviour
    {
        protected IProjectileSender? ProjectileSender;
        protected Vector2 Direction = Vector2.zero;
    
        protected CircleCollider2D? CircleCollider2D ;

        protected float TraveledDistance = 0f;
        protected SpriteRenderer SpriteRenderer = null!;

        protected float Damage;

        protected float Speed;

        protected bool DisposeOnDeath;
        private Animator _animator;


        protected virtual void Awake()
        {
            CircleCollider2D = GetComponent<CircleCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void FixedUpdate()
        {
            var v = Direction * (Time.fixedDeltaTime * Speed);
            transform.position += (Vector3) v;
            TraveledDistance += v.magnitude;
        }

        public virtual void Setup(Vector2 pos,Vector2 dir, IProjectileSender sender,  RangedStats stats, float speed=-1)
        {
            Damage = stats.AttackPower;
            Direction = dir;
            ProjectileSender = sender;
            Speed = Mathf.Approximately(speed, -1) ? stats.ProjectileSpeed : speed;
            DisposeOnDeath = stats.DisposeProjectilesOnDeath;
            var angle = math.atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0,0, angle);
            transform.position = pos;
            sender.SenderDispose += SenderOnSenderDispose;
            _animator.Play("Spawn");
        }

        private void SenderOnSenderDispose()
        {
            ProjectileSender = null;
            if(DisposeOnDeath)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (ProjectileSender != null) ProjectileSender.SenderDispose -= SenderOnSenderDispose;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(
                ProjectileSender?.GameObject
                    ? new DamageInfo(Damage, ProjectileSender.GameObject)
                    : new DamageInfo(Damage, null)
            );
            if (CircleCollider2D)
                CircleCollider2D.enabled = false;
            Direction *= 0;
            _animator.Play("Hit");
        }

        public void DestroyProjectile()
        {
            Destroy(gameObject);
        }


    }
}

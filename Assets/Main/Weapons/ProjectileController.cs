#nullable enable
using System;
using Main.Lib.Health;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        protected IProjectileSender ProjectileSender = null!;
        protected Vector2 Direction = Vector2.zero;
    
        protected CircleCollider2D? CircleCollider2D ;

        protected float TraveledDistance = 0f;
        protected SpriteRenderer SpriteRenderer = null!;

        protected float Damage;

        protected float Speed;


        protected virtual void Awake()
        {
            CircleCollider2D = GetComponent<CircleCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void FixedUpdate()
        {
            var v = Direction * (Time.fixedDeltaTime * Speed);
            transform.position += (Vector3) v;
            TraveledDistance += v.magnitude;
        }

        public void Setup(Vector2 pos,Vector2 dir, IProjectileSender sender,  float damage, float speed, bool disposeOnDeath)
        {
            Damage = damage;
            Direction = dir;
            ProjectileSender = sender;
            Speed = speed;
            var angle = math.atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0,0, angle);
            transform.position = pos;
            if(disposeOnDeath)
                sender.SenderDispose += SenderOnSenderDispose;
        }

        private void SenderOnSenderDispose()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            ProjectileSender.SenderDispose -= SenderOnSenderDispose;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(
                new DamageInfo(Damage, ProjectileSender.GameObject)
            );
            Destroy(gameObject,0.2f);
            if (CircleCollider2D)
                CircleCollider2D.enabled = false;
            SpriteRenderer.enabled = false;
            Direction *= 0;
        }


    }
}

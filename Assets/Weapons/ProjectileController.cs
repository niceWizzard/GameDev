#nullable enable
using Lib.Health;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] protected float speed = 5f;
        protected GameObject? ProjectileSender;
        protected Vector2 Direction = Vector2.zero;
    
        protected CircleCollider2D? CircleCollider2D ;

        protected float TraveledDistance = 0f;
        protected SpriteRenderer SpriteRenderer = null!;

        protected float Damage;

        protected virtual void Awake()
        {
            CircleCollider2D = GetComponent<CircleCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void FixedUpdate()
        {
            var v = Direction * (Time.fixedDeltaTime * speed);
            transform.position += (Vector3) v;
            TraveledDistance += v.magnitude;
        }

        public void Setup(Vector2 pos,Vector2 dir, GameObject sender,  float damage)
        {
            Damage = damage;
            ProjectileSender = sender;
            var angle = math.atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0,0, angle);
            transform.position = pos;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(
                new DamageInfo(Damage, ProjectileSender)
            );
            Destroy(gameObject,0.2f);
            if (CircleCollider2D)
                CircleCollider2D.enabled = false;
            SpriteRenderer.enabled = false;
            Direction *= 0;
        }


    }
}

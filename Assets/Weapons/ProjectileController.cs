#nullable enable
using Lib.Health;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private ParticleSystem hitParticles;
        private GameObject? _projectileSender;
        private Vector2 _direction = Vector2.zero;
    
        private CircleCollider2D? _circleCollider2D ;

        private float traveledDistance = 0f;
        private SpriteRenderer _spriteRenderer;
        private float _driftDirection;
        private int _accuracy = 2;
        private float _damage;

        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _driftDirection = (Random.Range(0, 1)) > 0 ? -1 : 1;
        }

        private void FixedUpdate()
        {
            var v = _direction * (Time.fixedDeltaTime * speed);
            transform.position += (Vector3) v;
            traveledDistance += v.magnitude;
        }

        public void Setup(Vector2 pos,Vector2 dir, GameObject sender, int accuracy, float damage)
        {
            _accuracy = accuracy;
            _damage = damage;
            
            var maxDriftAngle = ((11f - accuracy) / 10f) * 12f;
            var driftAngle = Random.Range(0.5f, maxDriftAngle);
            var driftDir = Random.Range(0, 2) * 2 - 1;
            _direction = Quaternion.Euler(0,0, driftAngle * driftDir) * dir.normalized;
            _projectileSender = sender;
            var angle = math.atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0,0, angle);
            transform.position = pos;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(
                new DamageInfo(_damage, _projectileSender)
            );
            Destroy(gameObject,0.2f);
            if (_circleCollider2D)
                _circleCollider2D.enabled = false;
            _spriteRenderer.enabled = false;
            _direction *= 0;
            hitParticles.Play();

        }


    }
}

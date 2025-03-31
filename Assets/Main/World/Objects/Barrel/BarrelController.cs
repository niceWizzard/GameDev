using Main.Lib.Health;
using UnityEngine;

namespace Main.World.Objects.Barrel
{
    public class BarrelController : MonoBehaviour, IDamageable
    {
        public HealthComponent HealthComponent { get; private set; }
        
        private SpriteRenderer SpriteRenderer { get; set; }
        private ParticleSystem _particleSystem;
        private Collider2D _collider;

        private void Awake()
        {
            HealthComponent = GetComponent<HealthComponent>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _particleSystem = GetComponent<ParticleSystem>();
            _collider = GetComponent<Collider2D>();
            HealthComponent.OnHealthZero += OnHealthZero;
        }
        private void OnDestroy()
        {
            HealthComponent.OnHealthZero -= OnHealthZero;
        }

        private void OnHealthZero()
        {
            SpriteRenderer.enabled = false;
            _collider.enabled = false;
            _particleSystem.Play();
            Destroy(gameObject, 1.5f);
        }

    }
}

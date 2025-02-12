using Main.Lib.Health;
using UnityEngine;

namespace World.Objects.Barrel
{
    public class BarrelController : MonoBehaviour, IDamageable
    {
        public HealthComponent HealthComponent { get; private set; }

        private SpriteRenderer SpriteRenderer { get; set; }
        private ParticleSystem _particleSystem;
        private void Awake()
        {
            HealthComponent = GetComponent<HealthComponent>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _particleSystem = GetComponent<ParticleSystem>();
            HealthComponent.OnHealthZero += OnHealthZero;
        }
        private void OnDestroy()
        {
            HealthComponent.OnHealthZero -= OnHealthZero;
        }

        private void OnHealthZero()
        {
            SpriteRenderer.enabled = false;
            _particleSystem.Play();
            Destroy(gameObject, 0.5f);
        }

    }
}

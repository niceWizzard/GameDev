using Main.Weapons.Gun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Weapons.Bullet
{
    public class BulletController : ProjectileController
    {
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private AudioSource awakeSound;
        private float _driftDirection;
        private int _accuracy = 2;

        protected override void Awake()
        {
            base.Awake();
            awakeSound.pitch = Random.Range(0.9f, 1f);
            awakeSound.Play();
            _driftDirection = (Random.Range(0, 1)) > 0 ? -1 : 1;
        }
    
        public override void Setup(Vector2 pos,Vector2 dir, IProjectileSender sender,  RangedStats stats, float speed=-1)
        {
            base.Setup(pos, dir, sender, stats,speed);
            _accuracy = stats.Accuracy;
            var maxDriftAngle = ((11f - _accuracy) / 10f) * 12f;
            var driftAngle = Random.Range(0.5f, maxDriftAngle);
            var driftDir = Random.Range(0, 2) * 2 - 1;
            Direction = Quaternion.Euler(0,0, driftAngle * driftDir) * dir.normalized;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            hitParticles.Play();
        }
    }
}

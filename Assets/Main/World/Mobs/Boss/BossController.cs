using Main.Lib.Health;
using Main.Lib.Mobs;
using Main.Weapons;
using Main.Weapons.Gun;
using Main.World.Mobs.Death_Animation;
using Unity.Cinemachine;
using UnityEngine;

namespace Main.World.Mobs.Boss
{
    public class BossController : EnemyController
    {
        [SerializeField] private ProjectileController projectilePrefab;
        [SerializeField] private RangedStats rangedStats;
        [SerializeField] private Hitbox bodyHitbox;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [Header("Boss Sfx")]
        [SerializeField] public AudioSource tackleCrashAudioSource;
        [SerializeField] public AudioSource laughAudioSource;
        public CinemachineImpulseSource CinemachineImpulseSource => impulseSource;
        
        public Vector2 ProjectileSpawn => projectileSpawnPoint.position;
        public ProjectileController ProjectilePrefab => projectilePrefab;
        public RangedStats RangedStats => rangedStats;
        public Hitbox BodyHitbox => bodyHitbox;

        protected override void Start()
        {
            base.Start();
            bodyHitbox.HurtboxHit += BodyHitboxOnHurtboxHit;
        }

        private void BodyHitboxOnHurtboxHit(Hurtbox obj)
        {
            obj.TakeDamage(new DamageInfo(RangedStats.AttackPower, gameObject));
        }

        protected override void OnDeathAnimation(DeathAnimation deathAnimation)
        {
            deathAnimation.Setup("BossDeath");
        }
    }
}

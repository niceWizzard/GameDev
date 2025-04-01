using System;
using Main.Lib.Health;
using Main.Lib.Mobs;
using Main.World.Mobs.Death_Animation;
using UnityEngine;

namespace Main.World.Mobs.Slime
{
    public class SlimeController : EnemyController
    {
        [SerializeField] private Hitbox hitbox;
        public Hitbox Hitbox => hitbox;

        protected override void Start()
        {
            base.Start();
            hitbox.Disable();
            hitbox.HurtboxHit += HitboxHit;
        }

        private void OnDestroy()
        {
            hitbox.HurtboxHit -= HitboxHit;
        }

        private void HitboxHit(Hurtbox hurtbox)
        {
            hurtbox.TakeDamage(new DamageInfo(Stats.AttackPower, gameObject));
        }

        public void SetFacingDirection()
        {
            if (Mathf.Abs(Velocity.x) > 0.1f)
            {
                SpriteRenderer.flipX = Mathf.Sign(Velocity.x) < 0; 
            }
        }


        protected override void OnDeathAnimation(DeathAnimation deathAnimation)
        {
            deathAnimation.Setup("SlimeDeath");
        }
    }
}

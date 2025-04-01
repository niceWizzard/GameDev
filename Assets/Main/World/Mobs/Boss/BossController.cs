using Main.Lib.Mobs;
using Main.Weapons;
using Main.Weapons.Gun;
using Main.World.Mobs.Death_Animation;
using UnityEngine;

namespace Main.World.Mobs.Boss
{
    public class BossController : EnemyController
    {
        [SerializeField] private ProjectileController projectilePrefab;
        [SerializeField] private RangedStats rangedStats;
        
        public ProjectileController ProjectilePrefab => projectilePrefab;
        public RangedStats RangedStats => rangedStats;

        protected override void OnDeathAnimation(DeathAnimation deathAnimation)
        {
            deathAnimation.Setup("GhostDeath");
        }
    }
}

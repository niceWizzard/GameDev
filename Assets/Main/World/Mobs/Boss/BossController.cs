using Main.Lib.Mobs;
using Main.World.Mobs.Death_Animation;
using UnityEngine;

namespace Main.World.Mobs.Boss
{
    public class BossController : EnemyController
    {
        

        protected override void OnDeathAnimation(DeathAnimation deathAnimation)
        {
            deathAnimation.Setup("GhostDeath");
        }
    }
}

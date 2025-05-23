using System;
using Main.Lib.Mobs;
using Main.Lib.Stat;
using Main.Weapons;
using Main.Weapons.Gun;
using Main.World.Mobs.Death_Animation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Mobs.Ghost
{
    public class GhostController : EnemyController
    {
        [SerializeField] private ProjectileController projectilePrefab;
            
        [SerializeField] private RangedStats rangedStats;
        [SerializeField] private Transform orbSpawn;
        
        public Vector2 OrbSpawn => orbSpawn.position;
        
        public ProjectileController ProjectilePrefab => projectilePrefab;
        public RangedStats RangedStats => rangedStats;

        public new Stats Stats => RangedStats;
        protected override void OnDeathAnimation(DeathAnimation deathAnimation)
        {
            deathAnimation.Setup("GhostDeath");
        }
    }
}

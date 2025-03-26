using System;
using Main.Lib.Mobs;
using Main.Lib.Stat;
using Main.Weapons;
using Main.Weapons.Gun;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Mobs.Ghost
{
    public class GhostController : EnemyController
    {
        [SerializeField] private ProjectileController projectilePrefab;
            
        [SerializeField] private RangedStats rangedStats;
        
        public ProjectileController ProjectilePrefab => projectilePrefab;
        public RangedStats RangedStats => rangedStats;

        public new Stats Stats => RangedStats;

    }
}

using System;
using Main.Lib.Mobs;
using Main.Lib.Stat;
using Main.Weapons;
using Main.Weapons.Gun;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class GhostController : EnemyController
    {
        [SerializeField] private ProjectileController projectilePrefab;
            
        [SerializeField] private GunnerStats gunnerStats;
        
        public ProjectileController ProjectilePrefab => projectilePrefab;
        public GunnerStats GunnerStats => gunnerStats;

        public new Stats Stats => GunnerStats;

    }
}

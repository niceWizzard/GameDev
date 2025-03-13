using Main.Lib.Mobs;
using Main.Weapons;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class GhostController : EnemyController
    {
        [SerializeField] private ProjectileController projectilePrefab;
        public ProjectileController ProjectilePrefab => projectilePrefab;

    }
}

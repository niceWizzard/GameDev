using CleverCrow.Fluid.UniqueIds;
using Main.Lib.Mobs;
using Main.Weapons;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GhostController : EnemyController
    {

        [SerializeField] private ProjectileController projectilePrefab;


        private UniqueId _uniqueId = null!;

        public ProjectileController ProjectilePrefab => projectilePrefab;

        public bool CanAttack { get; set; } = true;


        protected override void Awake()
        {
            base.Awake();
            _uniqueId = GetComponent<UniqueId>();
        }


        
    }
}

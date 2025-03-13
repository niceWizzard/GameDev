using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class GhostFsm : StateMachine<GhostFsm>
    {
        [SerializeField] private GhostController ghost;

        public bool ReachedPatrolPoint { get; set; }
        public bool CanPatrol { get; set; } = true;
        public Vector2 SpawnPoint { get; set; }
        
        private void Awake()
        {
            var idle = typeof(IdleState);
            var patrol = typeof(PatrolState);
            var states = new List<Type>()
            {
                idle,
                patrol
            };
            var transitions = new List<Transition>()
            {
                new(idle, patrol, () => CanPatrol),
                new(patrol, idle, () => ReachedPatrolPoint)
            };
            
            Setup(
                ghost.gameObject,
                states,
                transitions,    
                idle,
                this
            );
        }

        private void Start()
        {
            SpawnPoint = ghost.transform.position;
        }
    }
}

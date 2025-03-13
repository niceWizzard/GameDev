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
            var chase = typeof(ChaseState);
            var states = new List<Type>()
            {
                idle,
                patrol,
                chase,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, patrol, () => CanPatrol),
                Transition.Create(patrol, idle, () => ReachedPatrolPoint),
                Transition.MultiFrom(chase, () => ghost.detectedPlayer, patrol, idle),
                Transition.Create(chase, idle, () =>
                {
                    if (Vector2.Distance(ghost.detectedPlayer.transform.position,
                            ghost.transform.position) > 10f)
                    {
                        ghost.detectedPlayer = null;
                        return true;
                    }
                    return !ghost.detectedPlayer;
                }),
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

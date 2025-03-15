using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.World.Mobs.Ghost.States;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class GhostFsm : StateMachine<GhostFsm, GhostController>
    {
        [SerializeField] private GhostController ghost;

        public bool ReachedPatrolPoint { get; set; }
        public bool CanPatrol { get; set; } = true;
        public Vector2 SpawnPoint { get; set; }
        
        public bool CanAttack {get;set; } = true;

        private void Awake()
        {
            var idle = typeof(IdleState);
            var patrol = typeof(PatrolState);
            var chase = typeof(ChaseState);
            var attack = typeof(AttackState);
            var states = new List<Type>()
            {
                idle,
                patrol,
                chase,
                attack,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, patrol, () => CanPatrol),
                Transition.Create(patrol, idle, () => ReachedPatrolPoint),
                Transition.Create(chase, attack, () => CanAttack),
                Transition.Create(attack, chase, () => Vector2.Distance(ghost.Position, ghost.detectedPlayer.Position) > 4),
                Transition.MultiFrom(chase, () => ghost.detectedPlayer, patrol, idle),
                Transition.Create(chase, idle, () =>
                {
                    if (Vector2.Distance(ghost.detectedPlayer.Position,
                            ghost.Position) > 10f)
                    {
                        ghost.detectedPlayer = null;
                        return true;
                    }
                    return !ghost.detectedPlayer;
                }),
            };
            Setup(
                ghost,
                states,
                transitions,    
                idle,
                this
            );
        }

        private void Start()
        {
            SpawnPoint = ghost.Position;
        }
    }
}

using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.World.Mobs.Ghost.States;
using Unity.VisualScripting;
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
        public float PlayerTooClose { get; set; } = 0;

        private void Awake()
        {
            var idle = typeof(IdleState);
            var patrol = typeof(PatrolState);
            var chase = typeof(ChaseState);
            var attack = typeof(AttackState);
            var flee = typeof(FleeState);
            var states = new List<Type>()
            {
                idle,
                patrol,
                chase,
                attack,
                flee,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, patrol, () => CanPatrol),
                Transition.Create(patrol, idle, () => ReachedPatrolPoint),
                Transition.Create(chase, attack, () => CanAttack),
                Transition.Create(attack, chase, () => !ghost.detectedPlayer || Vector2.Distance(ghost.Position, ghost.detectedPlayer.Position) > 4),
                Transition.MultiFrom(chase, () => ghost.detectedPlayer, patrol, idle),
                Transition.Create(chase, idle, () =>
                {
                    if (!ghost.detectedPlayer)
                        return true;
                    if (Vector2.Distance(ghost.detectedPlayer.Position,
                            ghost.Position) > 10f
                    )
                    {
                        ghost.detectedPlayer = null;
                        return true;
                    }
                    return false;
                }),
                Transition.MultiFrom(flee, () =>
                {
                    if (!ghost.detectedPlayer)
                        return false;
                    var distance = Vector2.Distance(ghost.Position, ghost.detectedPlayer.Position);
                    switch (distance)
                    {
                        case < 2f:
                            PlayerTooClose = 4;
                            break;
                        case < 3:
                            PlayerTooClose += Time.deltaTime;
                            break;
                    } 
                    return PlayerTooClose >= 1;
                }, idle, patrol, chase),
                Transition.Create(flee, idle, () => !ghost.detectedPlayer || PlayerTooClose == 0),
            };
            Setup(
                ghost,
                states,
                transitions,    
                idle,
                this
            );
        }


        protected override void Update()
        {
            base.Update();
        }

        private void Start()
        {
            SpawnPoint = ghost.Position;
        }

        

    }
}

using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.World.Mobs.Slime.States;
using NavMeshPlus.Extensions;
using UnityEditor;
using UnityEngine;

namespace Main.World.Mobs.Slime
{
    public class SlimeFsm : StateMachine<SlimeFsm, SlimeController>
    {
        [SerializeField] private SlimeController slime;

        public const float AttackRange = 3f;
        public Vector2 SpawnPoint { get; set; }
        public bool ReachedPatrolPoint { get; set; }
        public bool CanPatrol { get; set; } = true;

        public bool AttackOnCd { get; set; } = false;

        public bool AttackFinished { get; set; } = false;
        public bool AttackWindupStarted { get; set; }
        public bool AttackWindupFinished { get; set; }

        private void Awake()
        {
            SpawnPoint = transform.position;
        }

        private void Start()
        {
            var idle = typeof(IdleState);
            var patrol = typeof(PatrolState);
            var chase = typeof(ChaseState);
            var attack = typeof(AttackState);
            var attackWindup = typeof(AttackWindupState);
            var states = new List<Type>()
            {
                idle,
                patrol,
                chase,
                attack,
                attackWindup,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, patrol, () => CanPatrol),
                Transition.Create(patrol, idle, () => ReachedPatrolPoint),
                Transition.MultiFrom(chase, () => slime.detectedPlayer, idle, patrol),
                Transition.Create(chase, idle, () => !slime.detectedPlayer),
                Transition.Create(chase, attackWindup, () => !AttackOnCd && slime.detectedPlayer && Vector2.Distance(slime.detectedPlayer.Position, slime.Position) < AttackRange),
                Transition.MultiFrom(chase, () =>
                {
                    if (slime.detectedPlayer 
                        && Vector2.Distance(slime.detectedPlayer.Position, slime.Position) >=
                        AttackRange + 2f)
                        return true;
                    return AttackFinished;
                }, attackWindup, attack),
                Transition.Create(attackWindup, attack, () => AttackWindupFinished),
            };
            
            Setup(
                slime,
                states,
                transitions,    
                idle,
                this
            );
        }

        
    }
}

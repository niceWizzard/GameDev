using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.World.Mobs.Boss.States;
using UnityEngine;

namespace Main.World.Mobs.Boss
{
    public class BossFsm : StateMachine<BossFsm, BossController>
    {
        [SerializeField] private BossController bossController;

        public bool PlayerInSweetSpot { get; set; } = true;
        
        public bool FleeStartDone { get; set; }
        public bool FleeFinishDone { get; set; }
        
        public bool AttackOnCd { get; set; }

        public bool AttackStateDone { get; set; } = true;

        public float ShouldFlee { get; set; } = 1;
        
        private void Start()
        {
        
            var idle = typeof(IdleState);
            var chill = typeof(RestState);
            var fleeStart = typeof(FleeStartState);
            var fleeFinish = typeof(FleeFinishState);
            var spawnOrbAttackState = typeof(SpawnOrbsState);
            var states = new List<Type>()
            {
                idle,
                chill,
                fleeStart,
                fleeFinish,
                spawnOrbAttackState,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, chill, () => bossController.detectedPlayer),
                Transition.Create(chill, idle, () => !bossController.detectedPlayer),
                Transition.MultiFrom(fleeStart, 
                    () => bossController.detectedPlayer && ShouldFlee <= 0, 
                chill, idle
                ),
                Transition.Create(fleeStart, fleeFinish, () => FleeStartDone),
                Transition.Create(fleeFinish, chill, () => FleeFinishDone),
                Transition.Create(chill, spawnOrbAttackState, () => PlayerInSweetSpot && !AttackOnCd),
                Transition.Create(spawnOrbAttackState, chill, () => AttackStateDone)
            };
            Setup(
                bossController,
                states,
                transitions,    
                idle,
                this
            );
        }

        private void LateUpdate()
        {
            Debug.Log(CurrentState);
        }
    }
}

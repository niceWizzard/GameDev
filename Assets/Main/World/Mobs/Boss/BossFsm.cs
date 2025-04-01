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
        
        private void Start()
        {
        
            var idle = typeof(IdleState);
            var chill = typeof(RestState);
            var fleeStart = typeof(FleeStartState);
            var fleeFinish = typeof(FleeFinishState);
            var states = new List<Type>()
            {
                idle,
                chill,
                fleeStart,
                fleeFinish
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, chill, () => bossController.detectedPlayer),
                Transition.MultiFrom(fleeStart, 
                    () => bossController.detectedPlayer &&
                                                 Vector2.Distance(bossController.Position, bossController.detectedPlayer.Position) < 1.2f, 
                chill, idle
                ),
                Transition.Create(fleeStart, fleeFinish, () => FleeStartDone),
                Transition.Create(fleeFinish, chill, () => FleeFinishDone),
            };
            Setup(
                bossController,
                states,
                transitions,    
                idle,
                this
            );
        }
    }
}

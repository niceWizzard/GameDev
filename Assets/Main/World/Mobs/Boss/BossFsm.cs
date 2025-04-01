using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.World.Mobs.Boss.States;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.World.Mobs.Boss
{
    public class BossFsm : StateMachine<BossFsm, BossController>
    {
        [SerializeField] private BossController bossController;

        public bool PlayerInSweetSpot { get; set; } = true;
        
        public bool FleeStartDone { get; set; }
        public bool FleeFinishDone { get; set; }
        
        public bool AttackOnCd { get; set; }
        public bool TackleOnCd { get; set; }
        public bool RampageOnCd { get; set; }
        public bool SummonOnCd { get; set; }

        public bool AttackStateDone { get; set; } = true;
        
        public bool TackleStateDone { get; set; } = true;
        public bool RampageStateDone { get; set; } = true;

        public float ShouldFlee { get; set; } = 1;
        public bool SummonStartStateDone { get; set; }

        private void Start()
        {
            var idle = typeof(IdleState);
            var chill = typeof(RestState);
            var fleeStart = typeof(FleeStartState);
            var fleeFinish = typeof(FleeFinishState);
            var tackle = typeof(TackleState);
            var spawnOrbAttackState = typeof(SpawnOrbsState);
            var rampage = typeof(RampageState);
            var summonStartState = typeof(SummonStartState);
            var states = new List<Type>()
            {
                idle,
                chill,
                fleeStart,
                fleeFinish,
                tackle,
                rampage,
                spawnOrbAttackState,
                summonStartState,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idle, chill, () => bossController.detectedPlayer),
                Transition.Create(Lib.FSM.States.AnyState, idle, () => !bossController.detectedPlayer),
                Transition.MultiFrom(fleeStart, 
                    () => bossController.detectedPlayer && ShouldFlee <= 0, 
                chill, idle
                ),
                Transition.Create(fleeStart, fleeFinish, () => FleeStartDone),
                Transition.Create(fleeFinish, chill, () => FleeFinishDone),
                
                // Summon
                Transition.Create(chill, summonStartState, () => !SummonOnCd && TackleOnCd && AttackOnCd
                                            && bossController.HealthComponent.HealthPercentage < .75f  
                                            &&Random.Range(0, 20) < 3 
                ),
                Transition.Create(summonStartState, fleeFinish, () => SummonStartStateDone),
                
                //Tackle
                Transition.Create(chill, tackle, () => 
                    AttackOnCd && !TackleOnCd && bossController.detectedPlayer 
                    && Vector2.Distance(bossController.detectedPlayer.Position, bossController.Position) > 3
                ),
                Transition.Create(tackle, chill, () => !bossController.detectedPlayer || TackleStateDone) ,
                // Spawn orb
                Transition.Create(chill, spawnOrbAttackState, () => PlayerInSweetSpot && !AttackOnCd),
                Transition.Create(spawnOrbAttackState, chill, () => AttackStateDone),
                
                // Rampage
                Transition.Create(chill, rampage, () => !RampageOnCd && bossController.HealthComponent.HealthPercentage < .5f ),
                Transition.Create(rampage, chill, () => RampageStateDone),
            };
            Setup(
                bossController,
                states,
                transitions,    
                idle,
                this
            );
        }

        
        public void SpawnProjectile(Vector2 dir, Vector2 position, float speed =-1)
        {
            
            var orb = Instantiate(bossController.ProjectilePrefab);
            orb.Setup(
                position,
                dir,
                bossController,
                bossController.RangedStats,
                speed
            );
        }
    }
}

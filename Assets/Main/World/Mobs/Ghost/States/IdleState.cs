using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class IdleState : State<GhostFsm, GhostController>
    {
        private float _patrolCd = 2;
        
        

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Idle");
            _patrolCd = 2;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Agent.Velocity = Vector2.Lerp(Agent.Velocity, Vector2.zero, 0.5f);
            _patrolCd -= Time.fixedDeltaTime;
            if (_patrolCd <= 0)
            {
                Executor.CanPatrol = true;
            }
        }
    }
}
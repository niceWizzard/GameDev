using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class IdleState : State<GhostFsm, GhostController>
    {
        private float _patrolCd = 2;
        


        public override void OnEnter()
        {
            base.OnEnter();
            _patrolCd = 2;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Agent.Velocity = Vector2.Lerp(Agent.Velocity, Vector2.zero, 0.5f);
            _patrolCd -= Time.deltaTime;
            if (_patrolCd <= 0)
            {
                Executor.CanPatrol = true;
            }
        }
    }
}
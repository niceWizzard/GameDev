using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class IdleState : State<GhostFsm>
    {
        private GhostController _ghost;
        private float _patrolCd = 2;
        

        public override void OnSetup(GameObject agent, GhostFsm executor)
        {
            base.OnSetup(agent, executor);
            _ghost = agent.GetComponent<GhostController>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _patrolCd = 2;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _ghost.Rigidbody2D.linearVelocity = Vector2.Lerp(_ghost.Rigidbody2D.linearVelocity, Vector2.zero, 0.5f);
            _patrolCd -= Time.deltaTime;
            if (_patrolCd <= 0)
            {
                Executor.CanPatrol = true;
            }
        }
    }
}
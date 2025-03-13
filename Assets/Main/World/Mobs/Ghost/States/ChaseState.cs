using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class ChaseState : State<GhostFsm>
    {
        private GhostController _ghost;
        

        public override void OnSetup(GameObject agent, GhostFsm executor)
        {
            base.OnSetup(agent, executor);
            _ghost = agent.GetComponent<GhostController>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _ghost.NavMeshAgent.SetDestination(_ghost.detectedPlayer.transform.position);
            var dir = ((Vector2)_ghost.NavMeshAgent.desiredVelocity).normalized;
            var vel = dir + _ghost.ContextBasedSteer(dir) * 0.5f * 0;
            _ghost.Rigidbody2D.linearVelocity = vel.normalized * _ghost.MovementSpeed;
        }
    }
}
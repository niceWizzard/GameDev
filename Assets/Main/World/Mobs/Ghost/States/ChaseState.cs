using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class ChaseState : State<GhostFsm, GhostController>
    {
        private int _drift;

        public override void OnEnter()
        {
            base.OnEnter();
            _drift = Random.Range(-45, 45);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Agent.NavMeshAgent.SetDestination(Agent.detectedPlayer.Position);
            var dir = ((Vector2)Agent.NavMeshAgent.desiredVelocity).normalized;
            var vel = dir + Agent.ContextBasedSteer(dir) * 0.5f * 0;
            Vector2 driftedDir = Quaternion.Euler(0,0, _drift) * vel;
            Agent.Velocity = driftedDir.normalized * Agent.MovementSpeed;
        }
    }
}
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class ChaseState : State<GhostFsm, GhostController>
    {

        public override void OnUpdate()
        {
            base.OnUpdate();
            Agent.NavMeshAgent.SetDestination(Agent.detectedPlayer.Position);
            var dir = ((Vector2)Agent.NavMeshAgent.desiredVelocity).normalized;
            var vel = dir + Agent.ContextBasedSteer(dir) * 0.5f * 0;
            Agent.Velocity = vel.normalized * Agent.MovementSpeed;
        }
    }
}
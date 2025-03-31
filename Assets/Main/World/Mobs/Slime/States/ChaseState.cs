using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Slime.States
{
    public class ChaseState : State<SlimeFsm, SlimeController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Chase");
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Agent.NavMeshAgent.SetDestination(Agent.detectedPlayer.Position);
            var dir = ((Vector2)Agent.NavMeshAgent.desiredVelocity).normalized;
            var vel = dir + Agent.ContextBasedSteer(dir) * 0.5f * 0;
            Agent.Velocity = vel.normalized * Agent.MovementSpeed;
            Agent.SetFacingDirection();
        }
    }
}
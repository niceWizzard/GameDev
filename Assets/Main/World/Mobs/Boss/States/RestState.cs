using Main.Lib.FSM;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.World.Mobs.Boss.States
{
    public class RestState : State<BossFsm, BossController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Move");
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var distance = Vector2.Distance(Agent.Position, Agent.detectedPlayer.Position);
            var toTarget = (Agent.detectedPlayer.Position - Agent.Position).normalized;
            var vel = (Agent.Velocity.normalized + Agent.ContextBasedSteer(toTarget, 2.5f) * 0.5f).normalized * Agent.MovementSpeed;
            Executor.PlayerInSweetSpot = false;
            switch (distance)
            {
                case < 1.5f:
                    Executor.ShouldFlee = 0;
                    break;
                case < 2:
                    Executor.ShouldFlee -= Time.fixedDeltaTime;
                    break;
                case < 3f:
                    Agent.Velocity = -vel;
                    break;
                case > 4f:
                    Agent.Velocity = vel;
                    break;
                default:
                    Agent.Velocity = Vector2.zero;
                    Executor.PlayerInSweetSpot = true;
                    break;
            }
        }

        
    }
}

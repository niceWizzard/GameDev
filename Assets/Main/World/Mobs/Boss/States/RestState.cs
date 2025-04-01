using Main.Lib.FSM;
using UnityEngine;

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
            Executor.PlayerInSweetSpot = false;
            switch (distance)
            {
                case < 3f:
                    MoveAway(-toTarget);
                    break;
                case > 4f:
                    MoveTowards(toTarget);
                    break;
                default:
                    Agent.Velocity = Vector2.zero;
                    Executor.PlayerInSweetSpot = true;
                    break;
            }
        }

        private void MoveAway(Vector2 dir)
        {
            Agent.Velocity = dir.normalized * Agent.MovementSpeed;
        }

        private void MoveTowards(Vector2 dir)
        {
            Agent.Velocity = dir.normalized * Agent.MovementSpeed;
        }
    }
}

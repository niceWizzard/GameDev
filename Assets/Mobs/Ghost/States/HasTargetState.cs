using Lib.StateMachine;
using UnityEngine;

namespace Mobs.Ghost.States
{
    public class HasTargetState : State<GhostController, GhostState>
    {
        public override void FixedDo()
        {
            base.FixedDo();
            if (!controller!.detectedPlayer)
            {
                ChangeState(GhostState.Patrolling);
                return;
            }
        
            Vector2 toTarget = (controller.detectedPlayer.transform.position - controller.transform.position);

            if (toTarget.magnitude > 12)
            {
                ChangeState(GhostState.Patrolling);
                return;
            }

            controller.Rigidbody2D.linearVelocity = toTarget.normalized * controller.MovementSpeed;

        }

        public override void Exit()
        {
            base.Exit();
            controller!.detectedPlayer = null;
        }
    }
}

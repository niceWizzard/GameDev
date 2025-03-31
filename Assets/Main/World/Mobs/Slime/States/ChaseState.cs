using Main.Lib.FSM;

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
            var toTarget = Agent.detectedPlayer.Position - Agent.Position;

            Agent.Velocity = toTarget.normalized * Agent.MovementSpeed;
            Agent.SetFacingDirection();
        }
    }
}
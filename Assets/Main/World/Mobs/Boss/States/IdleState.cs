using Main.Lib.FSM;

namespace Main.World.Mobs.Boss.States
{
    public class IdleState : State<BossFsm, BossController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Idle");
        }
    }
}
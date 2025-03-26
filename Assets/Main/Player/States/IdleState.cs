using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class IdleState : State<PlayerFsm, PlayerController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("IdleGun");

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Agent.RotateGun();
            Executor.ProcessAttackInputs();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Agent.Velocity = Vector2.zero;
        }
    }
}
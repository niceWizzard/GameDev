using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class IdleState : State<PlayerFsm>
    {
        private PlayerController controller;

        public override void OnSetup(Component agent, PlayerFsm executor)
        {
            base.OnSetup(agent, executor);
            this.controller  = agent.GetComponent<PlayerController>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            controller.Velocity = Vector2.zero;
            controller.RotateGun();
            Executor.ProcessAttackInputs();
        }
    }
}
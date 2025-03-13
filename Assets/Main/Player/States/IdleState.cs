using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class IdleState : State<PlayerFsm>
    {
        private PlayerController controller;

        public override void OnSetup(GameObject agent, PlayerFsm executor)
        {
            base.OnSetup(agent, executor);
            this.controller  = agent.GetComponent<PlayerController>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            controller.Rigidbody2d.linearVelocity = Vector2.zero;
            controller.RotateGun();
            Executor.ProcessAttackInputs();
        }
    }
}
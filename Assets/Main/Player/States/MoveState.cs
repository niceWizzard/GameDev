using Main.Lib.FSM;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Player.States
{
    public class MoveState : State<PlayerFsm>
    {
        private PlayerController _controller;
        public override void OnSetup(Component agent, PlayerFsm executor)
        {
            base.OnSetup(agent, executor);
            _controller  = agent.GetComponent<PlayerController>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Executor.ProcessAttackInputs();
            var input = _controller.GetMovementInput();
            _controller.UpdateFacingDirection(input);
            _controller.RotateGun();
            var vel = _controller.Velocity;
            vel = Vector2.Lerp(vel, input.normalized * _controller.MovementSpeed, _controller.friction);
            _controller.Velocity = vel;
        }
        
        
        
    }
}
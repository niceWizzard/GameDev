using Main.Lib.FSM;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Main.Player.States
{
    public class MoveState : State<PlayerFsm, PlayerController>
    {
        private Vector2 _input;
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("MoveGun");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Executor.ProcessAttackInputs();
            _input = Agent.GetMovementInput();
            Agent.RotateGun();
            
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var vel = Agent.Velocity;
            vel = Vector2.Lerp(vel, _input.normalized * Agent.MovementSpeed, Agent.friction);
            Agent.Velocity = vel;
        }
    }
}
using Main.Lib.FSM;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Player.States
{
    public class MoveState : State<PlayerFsm, PlayerController>
    {

        public override void OnUpdate()
        {
            base.OnUpdate();
            Executor.ProcessAttackInputs();
            var input = Agent.GetMovementInput();
            Agent.UpdateFacingDirection(input);
            Agent.RotateGun();
            var vel = Agent.Velocity;
            vel = Vector2.Lerp(vel, input.normalized * Agent.MovementSpeed, Agent.friction);
            Agent.Velocity = vel;
        }
        
        
        
    }
}
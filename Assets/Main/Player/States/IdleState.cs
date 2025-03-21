using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class IdleState : State<PlayerFsm, PlayerController>
    {


        public override void OnUpdate()
        {
            base.OnUpdate();
            Agent.Velocity = Vector2.zero;
            Agent.RotateGun();
            Executor.ProcessAttackInputs();
        }
    }
}
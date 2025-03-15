using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class NormalState : State<PlayerFsm, PlayerController>
    {
    public override void OnUpdate()
        {
            base.OnUpdate();
            if (!Agent)
                return ;
        
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ChangeState(PlayerState.Dash);
            }
            
            
        }

        
    }
}

using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class NormalState : State<PlayerFsm>
    {
        private PlayerController controller;

        public override void OnSetup(Component agent, PlayerFsm executor)
        {
            base.OnSetup(agent, executor);
            controller  = agent.GetComponent<PlayerController>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!controller)
                return ;
        
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ChangeState(PlayerState.Dash);
            }
            
            
        }

        
    }
}

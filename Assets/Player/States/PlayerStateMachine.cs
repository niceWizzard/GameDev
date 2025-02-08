using Lib.StateMachine;
using UnityEngine;

namespace Player.States
{
    public enum PlayerState
    {
        Normal,
        Dash,
    }

    public class PlayerStateMachine : StateMachine<PlayerController, PlayerState>
    {
        [SerializeField] private State<PlayerController, PlayerState> normalState;
        [SerializeField] private State<PlayerController, PlayerState> dashState;
    
        public override void InitializeStates()
        {
            StatesMap.Add(PlayerState.Dash, dashState);
            StatesMap.Add(PlayerState.Normal, normalState);
        }
    }
}
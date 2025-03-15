using System;
using System.Collections;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.Player.States;
using UnityEngine;

namespace Main.Player
{
    public class PlayerFsm : StateMachine<PlayerFsm>
    {
        [SerializeField] private PlayerController player;
        public bool DashFinished { get; set; } = true;

        private void Awake()
        {
            var idleState = typeof(IdleState);
            var dashState = typeof(Dash);
            var moveState = typeof(MoveState);

            var states = new List<Type>()
            {
                idleState,
                dashState,
                moveState,
            };

            var transitions = new List<Transition>()
            {
                Transition.Create(idleState, moveState, () => player.GetMovementInput().magnitude > 0),
                Transition.Create(moveState, idleState, () => player.GetMovementInput().magnitude == 0),
                Transition.Create(moveState, dashState, () => Input.GetKeyDown(KeyCode.Space)),
                Transition.Create(dashState, moveState, () => DashFinished),
            };
            
            Setup(player, states, transitions, idleState, this);
        }

        public void ProcessAttackInputs()
        {
            if (Input.GetMouseButton(0) )
            {
                player.Gun.NormalAttack();
            } else if (Input.GetMouseButtonDown(1))
            {
                player.Gun.SpecialAttack();
            } 
        }

    }
}

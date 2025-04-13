using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.Player.States;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Player
{
    public class PlayerFsm : StateMachine<PlayerFsm, PlayerController>
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

        protected override void Update()
        {
            if (Mathf.Approximately(Time.timeScale, 0))
                return;
            base.Update();
        }
        
        protected override void FixedUpdate()
        {
            if (Mathf.Approximately(Time.timeScale, 0))
                return;
            base.FixedUpdate();
        }

        public void ProcessAttackInputs()
        {
            if (!player.Gun.IsReloading && Input.GetKeyDown(KeyCode.R))
            {
                player.Gun.StartReload();
            }
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                player.Gun.NormalAttack();
            } else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                player.Gun.SpecialAttack();
            } 
        }
        
    }
}

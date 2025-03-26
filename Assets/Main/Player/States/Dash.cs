using System;
using System.Collections;
using System.Collections.Generic;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class Dash : State<PlayerFsm, PlayerController>
    {
        private Vector2 _direction = Vector2.zero;
        private float _traveledDistance = 0;
    
        

        private bool _isRecovering = false;


        
        public override void OnEnter()
        {
            base.OnEnter();
            var dir = Agent.GetMovementInput();
            _direction = dir.normalized;
            Executor.DashFinished = false;
            _traveledDistance = 0;
        }


        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (_isRecovering)
            {
                Agent.Velocity *= 0;
            }
            else
            {
                const float f = 10f;
                Agent.Velocity = _direction * f;
                _traveledDistance += f * Time.fixedDeltaTime;
            }
            if (_traveledDistance < Agent.dashDistance)
                return ;
            StartDashRecovery();
        }

        private void StartDashRecovery()
        {
            if (_isRecovering)
                return;
            Agent.StartCoroutine(_StartDashRecover());
        }

        private IEnumerator _StartDashRecover()
        {
            _isRecovering = true;
            yield return new WaitForSecondsRealtime(0.2f);
            Executor.DashFinished = true;
            _isRecovering = false;
        }
    }
}
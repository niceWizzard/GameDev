using System;
using System.Collections;
using System.Collections.Generic;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.Player.States
{
    public class Dash : State<PlayerFsm>
    {
        private Vector2 _direction = Vector2.zero;
        private float _traveledDistance = 0;
    
        
        private PlayerController _controller;

        private bool _isRecovering = false;

        public override void OnSetup(GameObject agent, PlayerFsm executor)
        {
            base.OnSetup(agent, executor);
            _controller  = agent.GetComponent<PlayerController>();
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            var dir = _controller.GetMovementInput();
            _direction = dir.normalized;
            Executor.DashFinished = false;
            _traveledDistance = 0;
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_isRecovering)
            {
                _controller.Rigidbody2d.linearVelocity *= 0;
            }
            else
            {
                const float f = 10f;
                _controller.Rigidbody2d.linearVelocity = _direction * f;
                _traveledDistance += f * Time.deltaTime;
            }
            if (_traveledDistance < _controller.dashDistance)
                return ;
            StartDashRecovery();
        }

        private void StartDashRecovery()
        {
            if (_isRecovering)
                return;
            _controller.StartCoroutine(_StartDashRecover());
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
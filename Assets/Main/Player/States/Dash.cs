using System;
using Main.Lib.StateMachine;
using UnityEngine;

namespace Main.Player.States
{
    internal enum DashStates
    {
        Invulnerable,
        Recovery,
    }

    public class Dash : State<PlayerController, PlayerState>
    {
        [SerializeField]
        private Color dashColor;
        private Vector2 direction = Vector2.zero;
        private float traveledDistance = 0;
    
        private DashStates _state = DashStates.Invulnerable;

        private float _recoveryTimer = 0;
    
        public override void Enter()
        {
            base.Enter();
            if (!controller)
            {
                Debug.LogError("Controller is not set!");
                return;
            }
            var dir = controller.GetMovementInput();
            if (dir.magnitude < 0.1f)
                dir = new Vector2(-controller.FacingDirection, 0);
            direction = dir.normalized;
            ChangeInternalState(DashStates.Invulnerable);
        }

        public override void Exit()
        {
            base.Exit();
            controller!.SpriteRenderer.color = Color.white;
        }

        private void ChangeInternalState(DashStates newState)
        {
            _state = newState;
            switch (_state)
            {
                case DashStates.Invulnerable:
                    controller!.SpriteRenderer.color = dashColor;
                    controller!.Hurtbox.Disable();
                    break;
                case DashStates.Recovery:
                    controller!.Hurtbox.Enable();
                    controller.SpriteRenderer.color = Color.white;
                    _recoveryTimer = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void FixedDo()
        {
            base.FixedDo();
            if (!controller) 
                return ;
            switch (_state)
            {
                case DashStates.Invulnerable:
                    const float f = 10f;
                    controller.Rigidbody2D.linearVelocity = direction * f;
                    traveledDistance += f * Time.fixedDeltaTime;
                    if (traveledDistance < controller.dashDistance)
                        return ;
                    traveledDistance = 0;
                    ChangeInternalState(DashStates.Recovery);
                    break;
                case DashStates.Recovery:
                    _recoveryTimer += Time.fixedDeltaTime;
                    controller.Rigidbody2D.linearVelocity *= 0;
                    if (_recoveryTimer < 0.1f)
                        return;
                    ChangeState(PlayerState.Normal);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
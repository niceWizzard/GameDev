using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Boss.States
{
    public class TackleState : State<BossFsm, BossController>
    {
        private float _traveled = 15;
        private Vector2 _fixedDirection;
        private float _time = 4;
        private bool _animationFinished = false;
        public override void OnEnter()
        {
            base.OnEnter();
            Executor.TackleStateDone = false;
            _animationFinished = false;
            Agent.Animator.Play("Tackle");
            Agent.laughAudioSource.Play();
            Agent.StartCoroutine(StartAnimationWaitTimer());
        }

        private IEnumerator StartAnimationWaitTimer()
        {
            yield return new WaitForSeconds(1.5f);
            _animationFinished = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            _traveled = 15;
            _time = 4;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!_animationFinished)
                return;
            var toTarget = (Agent.detectedPlayer.Position - Agent.Position); 
            if (_traveled > 10)
            {
                var dir = toTarget.normalized;
                Agent.Velocity = Vector2.Lerp(Agent.Velocity, dir * (Agent.MovementSpeed), 0.5f);
                _fixedDirection = dir;
            }
            else
            {
                Agent.Velocity = Vector2.Lerp(Agent.Velocity, _fixedDirection * (Agent.MovementSpeed * 5f), 0.5f);
            }
            _traveled -= Agent.Velocity.magnitude * Time.fixedDeltaTime;
            _time -= Time.fixedDeltaTime;
            var hit = Physics2D.CircleCast(Agent.Position, Agent.Collider2d.bounds.size.x / 2, Vector2.zero, 1,layerMask:  LayerMask.GetMask("World"));
            if (_time > 0 && _traveled > 0 && toTarget.magnitude > 0.4 && !hit.collider) return;
            if (hit.collider)
            {
                Agent.CinemachineImpulseSource.GenerateImpulse(2);
                Agent.tackleCrashAudioSource.Play();
            }
            Executor.TackleStateDone = true;
            Agent.Velocity *= 0;
            Agent.StartCoroutine(StartTackleCd());
        }

        private IEnumerator StartTackleCd()
        {
            Executor.TackleOnCd = true;
            yield return new WaitForSeconds(15);
            Executor.TackleOnCd = false;
        }
    }
}
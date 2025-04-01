using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Boss.States
{
    public class TackleState : State<BossFsm, BossController>
    {
        private float _traveled = 4;
        private Vector2 _fixedDirection;
        private float _time = 2;
        private bool _animationFinished = false;
        public override void OnEnter()
        {
            base.OnEnter();
            Executor.TackleStateDone = false;
            _animationFinished = false;
            Agent.Animator.Play("Tackle");
            Agent.StartCoroutine(StartAnimationWaitTimer());
        }

        private IEnumerator StartAnimationWaitTimer()
        {
            yield return new WaitForSeconds(0.5f);
            _animationFinished = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            _traveled = 4;
            _time = 2;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!_animationFinished)
                return;
            var toTarget = (Agent.detectedPlayer.Position - Agent.Position); 
            if (_traveled > 2)
            {
                var dir = toTarget.normalized;
                var multiplier = _traveled > 3 ? 1.5f : 3f;
                Agent.Velocity = Vector2.Lerp(Agent.Velocity, dir * (Agent.MovementSpeed * multiplier), 0.5f);
                _fixedDirection = dir;
            }
            else
            {
                Agent.Velocity = Vector2.Lerp(Agent.Velocity, _fixedDirection * (Agent.MovementSpeed * 3f), 0.5f);
            }
            _traveled -= Agent.Velocity.magnitude * Time.fixedDeltaTime;
            _time -= Time.fixedDeltaTime;
            if (_time > 0 && ( _traveled > 0 || toTarget.magnitude > 0.4)) return;
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
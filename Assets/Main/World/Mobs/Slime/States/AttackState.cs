using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Slime.States
{
    public class AttackState: State<SlimeFsm, SlimeController>
    {
        private Vector2 _dir;
        private float _travelDistance = 0;
        public override void OnEnter()
        {
            base.OnEnter();
            _dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
            Agent.Animator.Play("Attack");
            Agent.Hitbox.Enable();
        }

        public override void OnExit()
        {
            base.OnExit();
            Executor.AttackFinished = false;
            _travelDistance = 0;
            Executor.AttackOnCd = true;
            Agent.Hitbox.Disable();
            Executor.StartCoroutine(ResetAttackCd());
        }

        private IEnumerator ResetAttackCd()
        {
            yield return new WaitForSeconds(2f);
            Executor.AttackOnCd = false;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Agent.Velocity = _dir * 10f;
            _travelDistance += Agent.Velocity.magnitude * Time.fixedDeltaTime;
            if (_travelDistance >= SlimeFsm.AttackRange * 1.5f)
            {
                Executor.AttackFinished = true;
            }
        }
    }
}
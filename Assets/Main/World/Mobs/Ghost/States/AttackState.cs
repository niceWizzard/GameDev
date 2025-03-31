using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class AttackState : State<GhostFsm, GhostController>
    {
        private int _biasRotation;

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.StartCoroutine(Shoot());
            Executor.FinishedAttack = false;
            _biasRotation = Random.Range(-90, 90);
            Agent.Animator.Play("Attack");
            Agent.Velocity *= 0;
        }

        private IEnumerator Shoot()
        {
            Executor.CanAttack = false;
            yield return new WaitForSeconds(1);
            for (var i = 0; i < Agent.RangedStats.AmmoCapacity; i++)
            {
                if (!Agent || !Agent.detectedPlayer)
                {
                    Executor.CanAttack = true;
                    yield break;
                }
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                var projectile = Object.Instantiate(Agent.ProjectilePrefab, Agent.Position +  dir.normalized * 3, Quaternion.identity);
                projectile.Setup(Agent.Position, dir.normalized, Agent, Agent.RangedStats);
                yield return new WaitForSeconds(0.25f);
            }
            yield return StartAttackCdTimer();
        }

        public override void OnExit()
        {
            base.OnExit();
            Executor.FinishedAttack = true;
        }

        private IEnumerator StartAttackCdTimer()
        {
            yield return new WaitForSeconds(Agent.RangedStats.ReloadTime);
            Executor.CanAttack = true;
            Executor.FinishedAttack = true;
        }
    }
}
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
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Agent.NavMeshAgent.SetDestination(Agent.detectedPlayer.Position);
            var dir = ((Vector2)Agent.NavMeshAgent.desiredVelocity).normalized;
            var vel = dir + Agent.ContextBasedSteer(dir) * 0.5f * 0;
            Agent.Velocity = (Quaternion.Euler(0,0, _biasRotation) * vel).normalized * Agent.MovementSpeed;
        }

        private IEnumerator Shoot()
        {
            Executor.CanAttack = false;
            for (var i = 0; i < Agent.GunnerStats.AmmoCapacity; i++)
            {
                if (!Agent || !Agent.detectedPlayer)
                {
                    Executor.CanAttack = true;
                    yield break;
                }
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                var projectile = Object.Instantiate(Agent.ProjectilePrefab, Agent.Position +  dir.normalized * 3, Quaternion.identity);
                projectile.Setup(Agent.Position, dir.normalized, Agent, Agent.GunnerStats.AttackPower);
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
            yield return new WaitForSeconds(Agent.GunnerStats.ReloadTime);
            Executor.CanAttack = true;
            Executor.FinishedAttack = true;
        }
    }
}
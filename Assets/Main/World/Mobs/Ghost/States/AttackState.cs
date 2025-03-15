using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class AttackState : State<GhostFsm, GhostController>
    {
        
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.StartCoroutine(Shoot());
            Agent.Velocity *= 0;
        }

        private IEnumerator Shoot()
        {
            Executor.CanAttack = false;
            for (var i = 0; i < 3; i++)
            {
                if (!Agent || !Agent.detectedPlayer)
                {
                    Executor.CanAttack = true;
                    yield break;
                }
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                var projectile = Object.Instantiate(Agent.ProjectilePrefab, Agent.Position +  dir.normalized * 3, Quaternion.identity);
                projectile.Setup(Agent.Position, dir.normalized, Agent.gameObject, 20);
                yield return new WaitForSeconds(0.25f);
            }
            yield return StartAttackCdTimer();
        }

        private IEnumerator StartAttackCdTimer()
        {
            yield return new WaitForSeconds(2.5f);
            Executor.CanAttack = true;
        }
    }
}
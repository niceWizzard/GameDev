using System.Collections;
using Main.Lib.FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace Main.World.Mobs.Boss.States
{
    public class RampageState : State<BossFsm, BossController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Rampage"); 
            Agent.StartCoroutine(StartAttack());
            Executor.RampageStateDone = false;
        }

        private IEnumerator StartAttack()
        {
            yield return new WaitForSeconds(1.2f);

            yield return PlayAttackAnim();
            yield return Attack();

            Executor.RampageStateDone = true;
            Executor.RampageOnCd = true;
            yield return new WaitForSeconds(15);
            Executor.RampageOnCd = false;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Agent.Velocity *= 0;
        }

        private IEnumerator PlayAttackAnim()
        {
            Agent.Animator.Play("RampageAttack");
            yield return new WaitForSeconds(0.4f);
        }

        private IEnumerator Attack()
        {
            const float projectilePerWave = 10;
            const float angleStep = 360f / projectilePerWave; 
            
            for (var wave = 0; wave < 10; wave++)
            {
                if (!Agent.detectedPlayer)
                    yield break;
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                for (float angle = 0; angle < 360; angle += angleStep)
                {
                    var d = Quaternion.Euler(0,0, angle) * dir;
                    SpawnProjectile(d, Agent.ProjectileSpawn, 5);
                }
                yield return PlayAttackAnim();
            }
        }

        private void SpawnProjectile(Vector2 pos, Vector2 dir, float speed=-1) => Executor.SpawnProjectile(pos, dir, speed);
    }
}
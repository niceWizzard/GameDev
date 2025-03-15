using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class AttackState : State<GhostFsm>
    {
        private GhostController _ghost;

        public override void OnSetup(Component agent, GhostFsm executor)
        {
            base.OnSetup(agent, executor);
            _ghost = agent.GetComponent<GhostController>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _ghost.StartCoroutine(Shoot());
            _ghost.Velocity *= 0;
        }

        private IEnumerator Shoot()
        {
            Executor.CanAttack = false;
            for (var i = 0; i < 3; i++)
            {
                if (!_ghost || !_ghost.detectedPlayer)
                    yield break;
                var dir = (_ghost.detectedPlayer.Position - _ghost.Position).normalized;
                var projectile = Object.Instantiate(_ghost.ProjectilePrefab, _ghost.Position +  dir.normalized * 3, Quaternion.identity);
                projectile.Setup(_ghost.Position, dir.normalized, _ghost.gameObject, 20);
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
using System.Collections;
using Main.Lib.StateMachine;
using UnityEngine;

namespace Main.World.Mobs.Ghost.States
{
    public class HasTargetState : State<GhostController, GhostState>
    {
        public override void FixedDo()
        {
            base.FixedDo();
            if (!controller!.detectedPlayer)
            {
                ChangeState(GhostState.Patrolling);
                return;
            }
        
            Vector2 toTarget = (controller.detectedPlayer.transform.position - controller.transform.position);
            if (toTarget.magnitude > 12)
            {
                ChangeState(GhostState.Patrolling);
                return;
            }
            controller.Rigidbody2D.linearVelocity = toTarget.normalized * controller.MovementSpeed;

            if (!controller.CanAttack)
                return;
            
            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            if (controller ) 
                controller.CanAttack = false;
            for (var i = 0; i < 3; i++)
            {
                if (!controller || !controller.detectedPlayer)
                    break;
                Vector2 dir = (controller.detectedPlayer.transform.position - controller.transform.position).normalized;
                var projectile = Instantiate(controller!.ProjectilePrefab, controller.transform.position + (Vector3) dir.normalized * 3, Quaternion.identity);
                projectile.Setup(controller.transform.position, dir, controller.gameObject, 20);
                yield return new WaitForSeconds(0.25f);
            }
            yield return StartAttackCdTimer();
        }

        private IEnumerator StartAttackCdTimer()
        {
            yield return new WaitForSeconds(1.5f);
            controller.CanAttack = true;
        }


        public override void Exit()
        {
            base.Exit();
            controller!.detectedPlayer = null;
        }
    }
}

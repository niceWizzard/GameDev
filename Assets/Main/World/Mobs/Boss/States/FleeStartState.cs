using System.Collections;
using DG.Tweening;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Boss.States
{
    public class FleeStartState: State<BossFsm, BossController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Flee");
            Executor.FleeStartDone = false;
            Agent.Collider2d.enabled = false;
            Agent.Hurtbox.Disable();
            Agent.BodyHitbox.Disable();
            Agent.SpriteRenderer.DOFade(0, .5f).SetLink(Agent.gameObject);
            Agent.StartCoroutine(StartFleeTimer());
        }

        private IEnumerator StartFleeTimer()
        {
            yield return new WaitForSeconds(2);
            Executor.FleeStartDone = true;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var toPlayer = (Agent.detectedPlayer.Position - Agent.Position);
            var dir = -toPlayer.normalized; 
            Agent.Velocity = dir * (Agent.MovementSpeed * 2.5f);
        }


        public override void OnExit()
        {
            base.OnExit();
            Executor.FleeStartDone = false;
            Agent.Collider2d.enabled = true;
            Agent.Hurtbox.Enable();
            Agent.SpriteRenderer.DOFade(1, 1f).SetLink(Agent.gameObject);
        }
    }
}
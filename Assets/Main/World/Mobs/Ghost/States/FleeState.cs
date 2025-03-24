using DG.Tweening;
using Main.Lib.FSM;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Main.World.Mobs.Ghost.States
{
    public class FleeState : State<GhostFsm, GhostController>
    {
        private float timer = 2;
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Collider2d.enabled = false;
            Agent.Hurtbox.Disable();
            Agent.SpriteRenderer.DOFade(0, .5f).SetLink(Agent.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            Agent.Collider2d.enabled = true;
            Agent.Hurtbox.Enable();
            Agent.SpriteRenderer.DOFade(1, 1f).SetLink(Agent.gameObject);
            Executor.PlayerTooClose = 0;
            timer = 2;

            if (!Agent.detectedPlayer) return;
            var toPlayer = Agent.Position - Agent.detectedPlayer.Position;
            Agent.Position = -toPlayer.normalized * Random.Range(7, 12);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var toPlayer = (Agent.detectedPlayer.Position - Agent.Position);
            var dir = -toPlayer.normalized; 
            Agent.Velocity = dir * (Agent.MovementSpeed * 2.5f);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Executor.PlayerTooClose = 0;
            }
        }
    }
}
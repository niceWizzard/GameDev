using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Main.Lib.FSM;
using Main.Lib.Singleton;
using Main.World.Mobs.Ghost;
using UnityEngine;

namespace Main.World.Mobs.Boss.States
{
    public class SummonStartState : State<BossFsm, BossController>
    {
        private readonly List<GhostController> _summonedGhosts = new List<GhostController>();

        private bool _animFinished;
        public override void OnEnter()
        {
            base.OnEnter();
            _animFinished = false;
            Executor.SummonStartStateDone = false;
            Agent.Velocity *= 0;
            Executor.SummonOnCd = true;
            Agent.Animator.Play("Summon");
            Agent.StartCoroutine(StartAction());
        }

        public override void OnExit()
        {
            base.OnExit();
            _summonedGhosts.Clear();
        }



        private IEnumerator StartAction()
        {
            yield return new WaitForSeconds(1.5f);
            _animFinished = true;
            Agent.Hurtbox.Disable();
            Agent.BodyHitbox.Disable();
            Agent.Collider2d.enabled = false;
            var ghost1 = PrefabLoader.SpawnGhost(Agent.Position + Vector2.left * 2);
            var ghost2 = PrefabLoader.SpawnGhost(Agent.Position + Vector2.right * 2);
            _summonedGhosts.Add(ghost1);
            _summonedGhosts.Add(ghost2);
            yield return Agent.SpriteRenderer.DOFade(0, 0.5f).WaitForCompletion();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_animFinished)
                return;
            var toHeal = Agent.HealthComponent.MaxHealth * 0.015f;
            Agent.HealthComponent.Heal(toHeal * Time.deltaTime);
            if (_summonedGhosts.TrueForAll(v => !v))
            {
                Executor.SummonStartStateDone = true;
            }
        }
    }
}
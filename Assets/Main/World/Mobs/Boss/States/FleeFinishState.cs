using System;
using System.Collections;
using DG.Tweening;
using Main.Lib.FSM;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Main.World.Mobs.Boss.States
{
    public class FleeFinishState : State<BossFsm, BossController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Flee");
            Agent.Collider2d.enabled = true;
            Agent.Hurtbox.Enable();
            Agent.BodyHitbox.Enable();
            Agent.SpriteRenderer.DOFade(1, 1f).SetLink(Agent.gameObject);
            Agent.Velocity *= 0;
            Agent.Position = GetRandomPoint(Agent.detectedPlayer.Position, 9);
            Agent.StartCoroutine(StartTimer());
            Agent.StartCoroutine(StartSummonCdTimer());
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(1f);
            Executor.FleeFinishDone = true;
        }
        
        private IEnumerator StartSummonCdTimer()
        {
            if (!Executor.SummonOnCd)
                yield break;
            yield return new WaitForSeconds(30);
            Executor.SummonOnCd = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Executor.FleeFinishDone = false;
            Executor.ShouldFlee = 1;
        }

        private Vector2 GetRandomPoint(Vector2 origin, float radius = 3f)
        {
            var iteration = 0;
            while (iteration++ < 1000)
            {
                var angle = Random.Range(0f, 360f);
                var distance = Random.Range(3f, radius);
                Vector2 point = (Vector3)origin + Quaternion.Euler(0, 0, angle) * Vector2.right * distance;
                Agent.Collider2d.enabled = false;
                var hit = Physics2D.CircleCast(point, 0.5f, Vector2.zero, 0, Agent.dangerMask);
                Agent.Collider2d.enabled = true;
                if (hit) continue;
                if(!NavMesh.SamplePosition(point, out var navMeshHit,2, NavMesh.AllAreas))
                    continue;
                return point;
            }
            Debug.LogError("While Loop took too long!");
            return Vector2.zero;
        }
        
        
    }
}
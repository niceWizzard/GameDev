using System;
using System.Collections;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.Weapons;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Main.World.Mobs.Boss.States
{
    public class SpawnOrbsState : State<BossFsm, BossController>
    {
        private List<(float, string,Func<IEnumerator>)> attackPattern;

        

        public override void OnSetup()
        {
            base.OnSetup();
            attackPattern = new List<(float, string,Func<IEnumerator>)>
            {
                (30, "Attack2", TripleBulletSpawn),
                (5, "Attack2",ScatteredTripleBulletSpawn),
                (30, "Attack1", StraightSingleBulletSpawn),
                (5, "Attack1",ScatteredSingleBulletSpawn),
                (10, "Attack1",SpiralBulletSpawn)
            };
            Agent.HealthComponent.OnHealthChange += AgentHealthChange;
        }

        private void AgentHealthChange(float obj)
        {
            if (!Agent)
                return;
            var percent = Agent.HealthComponent.Health / Agent.HealthComponent.MaxHealth;
            if (percent > 0.7)
                return;
            attackPattern = new (){
                (20, "Attack2",TripleBulletSpawn),
                (15, "Attack2",ScatteredTripleBulletSpawn),
                (20, "Attack1",StraightSingleBulletSpawn),
                (15, "Attack1",ScatteredSingleBulletSpawn),
                (10, "Attack1",SpiralBulletSpawn)
            };
            Agent.HealthComponent.OnHealthChange -= AgentHealthChange;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Executor.AttackOnCd = true;
            Executor.AttackStateDone = false;
            Agent.StartCoroutine(StartSpawningOrbs());
        }

        private IEnumerator StartSpawningOrbs()
        {
            yield return RandomAttack();
            Executor.AttackStateDone = true;
            yield return new WaitForSeconds(2.5f);
            Executor.AttackOnCd = false;
        }

        private IEnumerator RandomAttack()
        {
            var roll = Random.Range(0,100f); 
            float cumulativeProbability = 0;
            foreach (var (prob, anim, action) in attackPattern)
            {
                cumulativeProbability += prob;
                if (!(roll <= cumulativeProbability)) continue;
                Agent.Animator.Play(anim);
                yield return new WaitForSeconds(1);
                yield return action?.Invoke();
                yield break;
            }
            yield return StraightSingleBulletSpawn();
        }

        private IEnumerator TripleBulletSpawn()
        {
            for (var i = 0; i < 10; i++)
            {
                if (!Agent.detectedPlayer)
                    break;
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                var perpendicular = new Vector2(-dir.y, dir.x); // Get a perpendicular vector
                for (var j = -1; j < 2; j++)
                {
                    var translate = perpendicular * (j * 0.8f);
                    SpawnProjectile( dir, Agent.ProjectileSpawn + translate);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator StraightSingleBulletSpawn()
        {
            for (var i = 0; i < 20; i++)
            {
                if (!Agent.detectedPlayer)
                    break;
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                SpawnProjectile(dir, Agent.ProjectileSpawn);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator ScatteredSingleBulletSpawn()
        {
            for (var i = 0; i < 20; i++)
            {
                if (!Agent.detectedPlayer)
                    break;
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                var driftAngle = Random.Range(-45f, 45f);
                SpawnProjectile(Quaternion.Euler(0,0,driftAngle) * dir, Agent.ProjectileSpawn, 6);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private IEnumerator ScatteredTripleBulletSpawn()
        {
            for (var i = 0; i < 10; i++)
            {
                if (!Agent.detectedPlayer)
                    break;
                var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
                var driftAngle = Random.Range(-45f, 45f);
                var perpendicular = new Vector2(-dir.y, dir.x); // Get a perpendicular vector
                for (var j = -1; j < 2; j++)
                {
                    var translate = perpendicular * (j * 0.8f);
                    SpawnProjectile(Quaternion.Euler(0,0,driftAngle) * dir, Agent.ProjectileSpawn + translate, 6);
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator SpiralBulletSpawn()
        {
            const float angleStep = 360f / 20; 
            
            var dir = (Agent.detectedPlayer.Position - Agent.Position).normalized;
            for (var wave = 0; wave < 20; wave++) 
            {
                var angle = wave * angleStep; 
                var d = Quaternion.Euler(0,0, angle) * dir;
                SpawnProjectile(d, Agent.ProjectileSpawn);
                SpawnProjectile(-d, Agent.ProjectileSpawn);
                yield return new WaitForSeconds(0.15f); // Delay between waves
            }
        }

        private void SpawnProjectile(Vector2 pos, Vector2 dir, float speed=-1) => Executor.SpawnProjectile(pos, dir, speed);
        
        
    }
}
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
        private List<(float, Func<IEnumerator>)> attackPattern;

        

        public override void OnSetup()
        {
            base.OnSetup();
            attackPattern = new List<(float, Func<IEnumerator>)>
            {
                (30, TripleBulletSpawn),
                (30, StraightSingleBulletSpawn),
                (5, ScatteredTripleBulletSpawn),
                (5, ScatteredSingleBulletSpawn),
                (10, SpiralBulletSpawn)
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
                (20, TripleBulletSpawn),
                (20, StraightSingleBulletSpawn),
                (15, ScatteredTripleBulletSpawn),
                (15, ScatteredSingleBulletSpawn),
                (10, SpiralBulletSpawn)
            };
            Agent.HealthComponent.OnHealthChange -= AgentHealthChange;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("Attack1");
            Executor.AttackOnCd = true;
            Executor.AttackStateDone = false;
            Agent.StartCoroutine(StartSpawningOrbs());
        }

        private IEnumerator StartSpawningOrbs()
        {
            yield return new WaitForSeconds(1);
            yield return RandomAttack();

            Executor.AttackStateDone = true;
            yield return new WaitForSeconds(2.5f);
            Executor.AttackOnCd = false;
        }

        private IEnumerator RandomAttack()
        {
            var roll = Random.Range(0,100f); 
            float cumulativeProbability = 0;
            foreach (var (prob, action) in attackPattern)
            {
                cumulativeProbability += prob;
                if (!(roll <= cumulativeProbability)) continue;
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
                    var translate = perpendicular * (j * 0.5f);
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
                SpawnProjectile(Quaternion.Euler(0,0,driftAngle) * dir, Agent.ProjectileSpawn);
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
                    var translate = perpendicular * (j * 0.5f);
                    SpawnProjectile(Quaternion.Euler(0,0,driftAngle) * dir, Agent.ProjectileSpawn + translate);
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
        
        private void SpawnProjectile(Vector2 dir, Vector2 position)
        {
            var orb = Object.Instantiate(Agent.ProjectilePrefab);
            orb.Setup(
                position,
                dir,
                Agent,
                Agent.RangedStats
            );
        }
        
        
    }
}
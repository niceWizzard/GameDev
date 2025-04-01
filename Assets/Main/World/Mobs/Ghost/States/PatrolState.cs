using System;
using Main.Lib.FSM;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Main.World.Mobs.Ghost.States
{
    public class PatrolState : State<GhostFsm, GhostController>
    {
        

        private Vector2 _targetPos;
        
        private int direction = 1;
        
        private Vector2 _targetPoint;
        


        public override void OnEnter()
        {
            base.OnEnter();
            Executor.ReachedPatrolPoint = false;
            _targetPoint = GetRandomPoint(Executor.SpawnPoint);
            Agent.NavMeshAgent.SetDestination(_targetPoint);
            Agent.Animator.Play("Move");
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var toTarget = (_targetPoint - (Vector2)Agent.Position);
            if (toTarget.magnitude < 0.1f)
            {
                Executor.ReachedPatrolPoint = true;
                Executor.CanPatrol = false;
                return;
            }

            var dir = ((Vector2)Agent.NavMeshAgent.desiredVelocity).normalized;
            
            var vel = dir + Agent.ContextBasedSteer(dir) * 0.5f;
            Agent.Velocity = vel.normalized * Agent.MovementSpeed;

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
            
            throw new Exception("While Loop took too long!");
        }
    }
}
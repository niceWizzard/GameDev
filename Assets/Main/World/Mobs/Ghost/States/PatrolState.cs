using System;
using Main.Lib.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.World.Mobs.Ghost.States
{
    public class PatrolState : State<GhostFsm>
    {
        private GhostController _ghost;

        private Vector2 _targetPos;
        
        private int direction = 1;
        
        private Vector2 _targetPoint;
        
        public override void OnSetup(Component agent, GhostFsm executor)
        {
            base.OnSetup(agent, executor);
            _ghost = agent.GetComponent<GhostController>();
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Executor.ReachedPatrolPoint = false;
            _targetPoint = GetRandomPoint(Executor.SpawnPoint);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var toTarget = (_targetPoint - (Vector2)_ghost.Position);
            if (toTarget.magnitude < 0.1f)
            {
                Executor.ReachedPatrolPoint = true;
                Executor.CanPatrol = false;
                return;
            }

            var dir = toTarget.normalized;
            
            var vel = dir + _ghost.ContextBasedSteer(dir) * 0.5f;
            _ghost.Velocity = vel.normalized * _ghost.MovementSpeed;

        }
        
        
        
        private Vector2 GetRandomPoint(Vector2 origin, float radius = 3f)
        {
            var iteration = 0;
            while (iteration++ < 1000)
            {
                var angle = Random.Range(0f, 360f);
                var distance = Random.Range(0.5f, radius);
                Vector2 point = (Vector3)origin + Quaternion.Euler(0, 0, angle) * Vector2.right * distance;
                _ghost.Collider2d.enabled = false;
                var hit = Physics2D.CircleCast(point, 0.5f, Vector2.zero, 0, _ghost.dangerMask);
                _ghost.Collider2d.enabled = true;
                if (hit) continue;
                return point;
            }

            throw new Exception("While Loop took too long!");
        }
    }
}
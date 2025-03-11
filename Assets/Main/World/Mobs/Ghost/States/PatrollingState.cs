using System;
using System.Collections;
using System.Linq;
using Main.Lib.StateMachine;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.World.Mobs.Ghost.States
{
    internal enum PatrolState
    {
        Idle,
        Walking,
    }
    public class PatrollingState : State<GhostController, GhostState>
    {
        const float PATROL_DISTANCE = 3f;
        private PatrolState _state = PatrolState.Walking;
        private int direction = 1;
        private Vector2 _spawnPoint;
        private Vector2 _targetPoint;
        public override void Prepare()
        {
            _spawnPoint = controller.transform.position;
            _targetPoint = GetRandomPoint(_spawnPoint);
        }

        public override void FixedDo()
        {
            base.FixedDo();
            switch (_state)
            {
                case PatrolState.Idle:
                    controller.Rigidbody2D.linearVelocity = Vector2.zero;
                    break;
                case PatrolState.Walking:
                    Vector2 toTarget = ((Vector3)_targetPoint - controller.transform.position);
                    var targetDir = toTarget.normalized;
                    var vel = targetDir + controller.ContextBasedSteer(targetDir) * 0.5f;
                    controller.Rigidbody2D.linearVelocity = vel.normalized * 2f;

                    if (toTarget.magnitude < 0.1f)
                    {
                        _state = PatrolState.Idle;
                        StartCoroutine(ChangePatrolPoint());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator ChangePatrolPoint()
        {
            yield return new WaitForSeconds(1);
            _targetPoint = GetRandomPoint(_spawnPoint);
            _state = PatrolState.Walking;
        }

        private Vector2 GetRandomPoint(Vector2 origin, float radius = 3f)
        {
            var iteration = 0;
            while (iteration++ < 1000)
            {
                var angle = Random.Range(0f, 360f);
                var distance = Random.Range(0.5f, radius);
                Vector2 point = (Vector3)origin + Quaternion.Euler(0, 0, angle) * Vector2.right * distance;
                controller.Collider2D.enabled = false;
                var hit = Physics2D.CircleCast(point, 0.5f, Vector2.zero, 0, controller.dangerMask);
                controller.Collider2D.enabled = true;
                if (hit) continue;
                return point;
            }

            throw new Exception("While Loop took too long!");
        }

        


    }
}
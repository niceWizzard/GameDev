using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;


enum PatrolState
{
    Idle,
    Walking,
}
public class PatrollingState : State<GhostController, GhostState>
{
    [SerializeField] private LayerMask dangerMask;
    const float PATROL_DISTANCE = 3f;
    private PatrolState _state = PatrolState.Walking;
    private int direction = 1;
    private Vector2 _spawnPoint;
    public override void Prepare()
    {
        _spawnPoint = controller.transform.position;
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
                var vel = controller.Rigidbody2D.linearVelocity.normalized + ContextBasedSteer(Vector2.right) * Time.fixedDeltaTime;
                controller.Rigidbody2D.linearVelocity = vel.normalized * 2f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Vector2 ContextBasedSteer(Vector2 desiredVelocity)
    {
        const int RAY_COUNT = 8;
        const float RAY_LENGTH = 1.5f;
        var rays = Enumerable.Range(0,RAY_COUNT).Select(i =>
        {
            var angle = (i * 2 * Mathf.PI / RAY_COUNT) * math.TODEGREES;
            Vector2 vec = Quaternion.Euler(0, 0, angle) * desiredVelocity;
            return vec.normalized;
        }).ToList();
        var interests = Enumerable.Range(0, RAY_COUNT).Select(i =>
        {
            var dot = Vector2.Dot(desiredVelocity.normalized, rays[i]);
            return dot;
        }).ToList();
        controller.Collider2D.enabled = false;
        for (var i = 0; i < rays.Count; i++)
        {
            var ray = rays[i];
            var hit = Physics2D.Raycast(controller.transform.position, ray, RAY_LENGTH, dangerMask);
            if (hit )
            {
                interests[i] -= 3f;
            }
        }
        controller.Collider2D.enabled = true;

        return rays.Select((v, i) => v * interests[i]).Aggregate(Vector2.zero, (current, next) => current + next).normalized;
    }

    private IEnumerator ChangePatrolDirection()
    {
        yield return new WaitForSeconds(1f);
        _state = PatrolState.Walking;
        _spawnPoint = controller.transform.position;
        direction *= -1;
    }
}

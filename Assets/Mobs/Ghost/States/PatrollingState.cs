using System;
using System.Collections;
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
                controller.Rigidbody2D.linearVelocity = Vector2.right * (direction * 2f);
                if (math.abs(_spawnPoint.x - controller.transform.position.x) > PATROL_DISTANCE)
                {
                    _state = PatrolState.Idle;
                    StartCoroutine(ChangePatrolDirection());
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator ChangePatrolDirection()
    {
        yield return new WaitForSeconds(1f);
        _state = PatrolState.Walking;
        _spawnPoint = controller.transform.position;
        direction *= -1;
    }
}

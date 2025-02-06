using Unity.Mathematics;
using UnityEngine;


public class Dash : State<PlayerController, PlayerState>
{
    private Vector2 direction = Vector2.zero;
    private float traveledDistance = 0;
    
    public override void Enter()
    {
        base.Enter();
        if (!controller)
        {
            Debug.LogError("Controller is not set!");
            return;
        }
        var dir = controller.GetMovementInput();
        if (dir.magnitude < 0.1f)
            dir = new Vector2(-controller.FacingDirection, 0);
        direction = dir.normalized;
    }

    public override void FixedDo()
    {
        base.FixedDo();
        if (!controller) return ;

        const float f = 10f;
        controller.rigidbody2d.linearVelocity = direction * f;
        traveledDistance += f * Time.fixedDeltaTime;
        if (traveledDistance < controller.dashDistance)
            return ;
        traveledDistance = 0;
        ChangeState(PlayerState.Normal);
    }
}

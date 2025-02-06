using Unity.Mathematics;
using UnityEngine;


public class Dash : State<PlayerController>
{
    private Vector2 direction = Vector2.zero;
    private float traveledDistance = 0;
    
    [SerializeField] private State<PlayerController> normalState;
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

    public override State<PlayerController> FixedDo()
    {
        var a = base.FixedDo();
        if (a)
            return a;
        if (!controller) return null;

        const float f = 10f;
        controller.rigidbody2d.linearVelocity = direction * f;
        traveledDistance += f * Time.fixedDeltaTime;
        if (traveledDistance < controller.dashDistance)
            return null;
        traveledDistance = 0;
        return normalState;
    }
}

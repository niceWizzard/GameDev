using Unity.Mathematics;
using UnityEngine;

public class Normal : State<PlayerController>
{
    public override State<PlayerController> FixedDo()
    {
        var a = base.FixedDo();
        if (a)
            return a;

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!controller) return null;
        controller.UpdateFacingDirection(input);
        
        var vel = controller.rigidbody2d.linearVelocity;
        vel = Vector2.Lerp(vel, input.normalized * controller.movementSpeed, controller.friction);
        controller.rigidbody2d.linearVelocity = vel;
        return null;
    }
}

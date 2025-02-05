using UnityEngine;

public class Normal : State<PlayerController>
{
    public override State<PlayerController> FixedDo()
    {
        var a = base.FixedDo();
        if (a != null)
            return a;

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Debug.Log(input);
        controller.rigidbody2d.linearVelocity = input.normalized * 5.5f;
        
        return null;
    }
}

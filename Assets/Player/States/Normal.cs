using System;
using Unity.Mathematics;
using UnityEngine;

public class Normal : State<PlayerController>
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public override State<PlayerController> FixedDo()
    {
        var a = base.FixedDo();
        if (a)
            return a;

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!controller) return null;
        controller.UpdateFacingDirection(input);
        RotateGun();
        var vel = controller.rigidbody2d.linearVelocity;
        vel = Vector2.Lerp(vel, input.normalized * controller.movementSpeed, controller.friction);
        controller.rigidbody2d.linearVelocity = vel;
        return null;
    }

    void RotateGun()
    {
        if (!controller ||  !_camera)
            return;
        var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = (mouse - controller.transform.position).normalized;
        var angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
        controller.GunAnchor.localEulerAngles = new Vector3(0, 0, angle);
        controller.Gun.transform.localScale = math.abs(angle) > 90 ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
    }
}

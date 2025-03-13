using Main.Lib.StateMachine;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Player.States
{
    public class Normal : State<PlayerController, PlayerState>
    {
        private Camera _camera;


        private void Start()
        {
            _camera = Camera.main;
        }

        public override void Do()
        {
            base.Do();
            if (!controller)
                return ;
        
            if (Input.GetMouseButton(0) )
            {
                controller.Gun.NormalAttack();
            } else if (Input.GetMouseButtonDown(1))
            {
                controller.Gun.SpecialAttack();
            } 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.Dash);
            }
        }

        public override void FixedDo()
        {
            base.FixedDo();
            var input = controller.GetMovementInput();
            if (!controller) return ;
            controller.UpdateFacingDirection(input);
            RotateGun();
            var vel = controller.Rigidbody2D.linearVelocity;
            vel = Vector2.Lerp(vel, input.normalized * controller.MovementSpeed, controller.friction);
            controller.Rigidbody2D.linearVelocity = vel;
        }

        private void RotateGun()
        {
            if (!controller ||  !_camera)
                return;
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = (mouse - controller.transform.position).normalized;
            var angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
            controller.GunAnchor.localEulerAngles = new Vector3(0, 0, angle);
            controller.Gun.FlipSprite(math.abs(angle) > 90);
        }
    }
}

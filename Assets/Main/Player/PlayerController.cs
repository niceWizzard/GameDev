using DG.Tweening;
using Main.Lib;
using Main.Lib.Health;
using Main.Weapons.Gun;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Player
{
    public class PlayerController : MobController, IDamageable
    {
        [Header("Stats")]
        public float friction = 0.5f;
        public float dashDistance = 1f;
        [Header("Components")]
        [SerializeField]
        private GunController gun;

        [SerializeField] private TMP_Text reloadingText;

        [SerializeField] private Transform gunAnchor;
        
        private Camera _camera;
        public GunController Gun => gun;
        public Transform GunAnchor => gunAnchor;
        public float FacingDirection { get; private set; } = 1;


        private void Start()
        {
            gun.OnReloadStart += GunOnReloadStart;
            gun.OnReloadEnd += GunOnReloadEnd;
            reloadingText.color = new Vector4(0,0,0,0);
            _camera = Camera.main;
        }

        private void GunOnReloadEnd()
        {
            reloadingText.DOColor(new Vector4(0,0,0,0), 0.2f);
        }

        private void GunOnReloadStart()
        {
            reloadingText.DOColor(Color.white, 0.3f);
        }

        public void UpdateFacingDirection(Vector2 input)
        {
            if (math.abs(input.x) < 0.01f)
                return;
            FacingDirection = math.sign(input.x);
            SpriteRenderer.flipX = FacingDirection < 0;
        }

        public Vector2 GetMovementInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
        public void RotateGun()
        {
            if (!_camera)
                return;
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = (mouse - transform.position).normalized;
            var angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
            GunAnchor.localEulerAngles = new Vector3(0, 0, angle);
            Gun.FlipSprite(math.abs(angle) > 90);
        }
    }
}

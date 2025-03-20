using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Health;
using Main.Weapons.Gun;
using TMPro;
using Unity.Cinemachine;
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
        private CinemachineImpulseSource _impulseSource;
        public GunController Gun => gun;
        public Transform GunAnchor => gunAnchor;
        public float FacingDirection { get; private set; } = 1;
        
        public bool InHurtAnimation { get; private set; }

        private void Start()
        {
            gun.OnReloadStart += GunOnReloadStart;
            gun.OnReloadEnd += GunOnReloadEnd;
            reloadingText.color = new Vector4(0,0,0,0);
            _camera = Camera.main;
        }

        protected override void GetRequiredComponents()
        {
            base.GetRequiredComponents();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        protected override void VerifyRequiredComponents()
        {
            base.VerifyRequiredComponents();
            if(!_impulseSource)
                Debug.LogError($"Impulse Source is not set at {name}");
        }

        protected override void OnHurtboxHurt(DamageInfo damageInfo)
        {
            base.OnHurtboxHurt(damageInfo);
            _ = StartHurtAnimation();
        }

        private async UniTask StartHurtAnimation()
        {
            if (InHurtAnimation)
                return;
            _impulseSource.GenerateImpulseWithForce(4);
            InHurtAnimation = true;
            SetVisibility(true);
            Hurtbox.Disable();
            for (var i = 0; i < 5; i++)
            {
                SetVisibility(!SpriteRenderer.enabled);
                await UniTask.WaitForSeconds(0.2f);
            }
            for (var i = 0; i < 3; i++)
            {
                await UniTask.WaitForSeconds(0.3f);
                SetVisibility(!SpriteRenderer.enabled);
            }
            Hurtbox.Enable();
            SetVisibility(true);
            InHurtAnimation = false;
        }

        public void SetVisibility(bool visible)
        {
            SpriteRenderer.enabled = visible;
            Gun.SpriteRenderer.enabled = visible;
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

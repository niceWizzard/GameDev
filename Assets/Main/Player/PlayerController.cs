using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Health;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;
using Main.Weapons.Gun;
using Main.World.Mobs.Death_Animation;
using TMPro;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor.Analytics;
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
        [SerializeField] private RangedStats rangedStats;

        [SerializeField] private Transform leftHandPosition;
        [SerializeField] private Transform rightHandPosition;

        [SerializeField] private ReloadAnimation reloadAnimationController;

        [SerializeField] private Transform gunAnchor;
        [SerializeField] private Hurtbox feetHurtbox;
        [SerializeField] private AudioSource walkAudioSource;
        
        private Camera _camera;
        private CinemachineImpulseSource _impulseSource;
        
        public AudioSource WalkAudioSource => walkAudioSource;
        public GunController Gun => gun;
        public Transform GunAnchor => gunAnchor;
        public float FacingDirection { get; private set; } = 1;
        
        public bool InHurtAnimation { get; private set; }

        public static event Action<RangedStats> StatChange;

        protected override void Awake()
        {
            base.Awake();
            MainCamera.Instance.Follow(this);
            HUDController.Instance.SetPlayer(this);
        }

        private void Start()
        {
            gun.OnReloadStart += GunOnReloadStart;
            feetHurtbox.OnHurt += OnHurtboxHurt;
            _camera = Camera.main;
            LoadStats();
            HealthComponent.SetMaxHealth(rangedStats.Health);
        }

        

        public void LoadStats()
        {
            rangedStats.SetFromSave(SaveManager.Instance.SaveGameData.StatUpgrades);
            Gun.UpdateAmmoCapacity(rangedStats.AmmoCapacity);
            StatChange?.Invoke(rangedStats);
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


        protected override async UniTask HurtAnimation()
        {
            if (InHurtAnimation)
                return;
            _ = base.HurtAnimation();
            _impulseSource.GenerateImpulse(2.5f);
            InHurtAnimation = true;
            SetVisibility(true);
            Hurtbox.Disable();
            feetHurtbox.Disable();
            Time.timeScale = 0;
            try
            {
                await UniTask.WaitForSeconds(0.05f, ignoreTimeScale: true, cancellationToken: destroyCancellationToken);
            }
            catch (OperationCanceledException e)
            {
                Time.timeScale = 1;
                return;
            }
            Time.timeScale = 1;
            try
            {
                for (var i = 0; i < 3; i++)
                {
                    SetVisibility(!SpriteRenderer.enabled);
                    await UniTask.WaitForSeconds(0.1f, cancellationToken: destroyCancellationToken);
                }
                for (var i = 0; i < 3; i++)
                {
                    await UniTask.WaitForSeconds(0.2f, cancellationToken: destroyCancellationToken);
                    SetVisibility(!SpriteRenderer.enabled);
                }
            }
            catch (OperationCanceledException e)
            {
                return;
            }
            Hurtbox.Enable();
            feetHurtbox.Enable();
            SetVisibility(true);
            InHurtAnimation = false;
        }

        protected override void OnDeathAnimation(DeathAnimation deathAnimation)
        {
            deathAnimation.Setup("PlayerDeath");
        }

        public void SetVisibility(bool visible)
        {
            SpriteRenderer.enabled = visible;
            Gun.SpriteRenderer.enabled = visible;
        }


        private void GunOnReloadStart()
        {
            reloadAnimationController.StartAnimation(rangedStats.ReloadTime);
        }

        public void UpdateFacingDirection(int direction)
        {
            FacingDirection = direction;
            SpriteRenderer.flipX = FacingDirection < 0;
        }

        public Vector2 GetMovementInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
        public void RotateGun()
        {
            if (!_camera || Time.timeScale == 0) 
                return;
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = (mouse - transform.position).normalized;
            var angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
            var facing = Math.Abs(angle) >90 ? -1 : 1;
            UpdateFacingDirection(facing);
            GunAnchor.position = facing == 1 ? rightHandPosition.position : leftHandPosition.position;
            GunAnchor.localEulerAngles = new Vector3(0, 0, angle);
            Gun.FlipSprite(math.abs(angle) > 90);
        }
    }
}

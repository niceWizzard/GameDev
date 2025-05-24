using System;
using System.Collections;
using Main.Lib.Singleton;
using Main.Weapons.Bullet;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Weapons.Gun
{
    public class GunController : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private RangedStats ownerStats;
        [SerializeField] private Transform leftNozzleTransform;
        [SerializeField] private Transform rightNozzleTransform;
        [SerializeField] private GameObject owner;
        [SerializeField, Space(10)]
        private BulletController normalAttackPrefab;
        
        [SerializeField]
        private BulletController specialAttackPrefab;
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public event Action OnReloadStart;
        public event Action OnReloadEnd;
        public event Action<int> OnAmmoUsed;
        

        public IProjectileSender Owner { get; private set; }
        private Transform NozzleTransform => _spriteRenderer.flipY ? rightNozzleTransform : leftNozzleTransform;

        private int _currentAmmo;
        public int AmmoCapacity => ownerStats.AmmoCapacity;

        public int CurrentAmmo
        {
            get => _currentAmmo;
            private set
            {
                _currentAmmo = value;
                OnAmmoUsed?.Invoke(value);
            }
        }
        

        private bool _canShoot = true;
        public bool IsReloading { get; private set; } = false;
        private CinemachineImpulseSource _impulseSource;
        private float AttackCd => 1f / ownerStats.AttackPerSecond;
        private void Awake()
        {
            _camera = MainCamera.Instance?.Camera;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            CurrentAmmo = ownerStats.AmmoCapacity;
            Owner = owner.GetComponent<IProjectileSender>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void AddCameraRecoil(Vector2 direction)
        {
            _impulseSource.GenerateImpulse(direction.normalized * .15f);
        }


        public void FlipSprite(bool isFlipped)
        {
            _spriteRenderer.flipY = isFlipped;
        }

        public void NormalAttack(Vector2? dir=null)
        {
            if (!_camera || !_canShoot || IsReloading)
                return;
            var projectile = Instantiate(normalAttackPrefab, transform.position, Quaternion.identity);
            _canShoot = false;
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mouse - Owner.GameObject.transform.position).normalized;
            projectile.Setup(NozzleTransform.position,dir ?? mouseDir, Owner , ownerStats);
            AddCameraRecoil(-mouseDir);
            StartCoroutine(--CurrentAmmo <= 0 ? StartReloadTimer() : StartAttackCdTimer());
        }

        public void SpecialAttack()
        {
            if (!_camera || !_canShoot || IsReloading || CurrentAmmo < 5)
                return;
            var projectile = Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mouse - Owner.GameObject.transform.position).normalized;
            projectile.Setup(NozzleTransform.position,mouseDir, Owner , ownerStats);
            CurrentAmmo -= 5;
            AddCameraRecoil(-mouseDir);
            StartCoroutine(CurrentAmmo <= 0 ? StartReloadTimer() : StartAttackCdTimer());
        }

        public void UpdateAmmoCapacity(int newCapacity)
        {
            CurrentAmmo = newCapacity;
            
        }

        public void StartReload()
        {
            StartCoroutine(StartReloadTimer());
        }
        
        private IEnumerator StartReloadTimer()
        {
            IsReloading = true;
            CurrentAmmo = 0;
            OnReloadStart?.Invoke();
            yield return new WaitForSeconds(ownerStats.ReloadTime);
            OnReloadEnd?.Invoke();
            IsReloading = false;
            CurrentAmmo = ownerStats.AmmoCapacity;
            _canShoot = true;
        }
        
        private IEnumerator  StartAttackCdTimer()
        {
            yield return new WaitForSeconds(AttackCd);
            _canShoot = true;
        }
    }
}

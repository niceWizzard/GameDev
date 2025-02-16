using System;
using System.Collections;
using Main.Lib.Singleton;
using Main.Weapons.Bullet;
using UnityEngine;

namespace Main.Weapons.Gun
{
    public class GunController : MonoBehaviour
    {
        [Header("Stats")] 
        [SerializeField, Range(3, 15)] private int attackPerSecond = 4;
        [SerializeField] private int ammoCapacity = 7;
        [SerializeField, Range(4, 10)] private int accuracy = 4;
        [SerializeField] private int normalAttackDamage = 50;
        [SerializeField] private int specialAttackDamage = 125;
        [Header("Components")]
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
        
        

        public GameObject Owner => owner;
        private Transform NozzleTransform => _spriteRenderer.flipY ? rightNozzleTransform : leftNozzleTransform;

        private int _currentAmmo;
        public int AmmoCapacity => ammoCapacity;

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
        private bool _isReloading = false;
        private float AttackCd => 1f / attackPerSecond;
        private void Start()
        {
            _camera = MainCamera.Instance.Camera;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            CurrentAmmo = ammoCapacity;
        }

        public void FlipSprite(bool isFlipped)
        {
            _spriteRenderer.flipY = isFlipped;
        }

        public void NormalAttack()
        {
            if (!_camera || !_canShoot || _isReloading)
                return;
            var projectile = Instantiate(normalAttackPrefab, transform.position, Quaternion.identity);
            _canShoot = false;
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mouse - Owner.transform.position).normalized;
            projectile.Setup(NozzleTransform.position,mouseDir, Owner ,normalAttackDamage, accuracy);
            StartCoroutine(--CurrentAmmo <= 0 ? StartReloadTimer() : StartAttackCdTimer());
            
        }

        public void SpecialAttack()
        {
            if (!_camera || !_canShoot || _isReloading || CurrentAmmo < 5)
                return;
            var projectile = Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mouse - Owner.transform.position).normalized;
            projectile.Setup(NozzleTransform.position,mouseDir, Owner ,specialAttackDamage, accuracy);
            CurrentAmmo -= 5;
            StartCoroutine(CurrentAmmo <= 0 ? StartReloadTimer() : StartAttackCdTimer());
        }

        private IEnumerator StartReloadTimer()
        {
            _isReloading = true;
            CurrentAmmo = 0;
            OnReloadStart?.Invoke();
            yield return new WaitForSeconds(2.5f);
            OnReloadEnd?.Invoke();
            _isReloading = false;
            CurrentAmmo = ammoCapacity;
            _canShoot = true;
        }

        private IEnumerator  StartAttackCdTimer()
        {
            yield return new WaitForSeconds(AttackCd);
            _canShoot = true;
        }
    }
}

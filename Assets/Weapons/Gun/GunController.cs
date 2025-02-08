using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons.Gun
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
        private ProjectileController normalAttackPrefab;
        [SerializeField]
        private ProjectileController specialAttackPrefab;
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public GameObject Owner => owner;
        private Transform NozzleTransform => _spriteRenderer.flipY ? rightNozzleTransform : leftNozzleTransform;

        private int _ammoCount;
        private bool _canShoot = true;
        private bool _isReloading = false;
        private float AttackCd => 1f / attackPerSecond;
        private void Awake()
        {
            _camera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _ammoCount = ammoCapacity;
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
            projectile.Setup(NozzleTransform.position,mouseDir, Owner ,accuracy, normalAttackDamage);
            StartCoroutine(--_ammoCount <= 0 ? StartReloadTimer() : StartAttackCdTimer());
        }

        public void SpecialAttack()
        {
            if (!_camera || !_canShoot || _isReloading || _ammoCount < 3)
                return;
            var projectile = Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);
            _canShoot = false;
            var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mouse - Owner.transform.position).normalized;
            projectile.Setup(NozzleTransform.position,mouseDir, Owner ,accuracy, specialAttackDamage);
            StartCoroutine(StartReloadTimer());
        }

        private IEnumerator StartReloadTimer()
        {
            yield return new WaitForSeconds(2.5f);
            _isReloading = false;
            _ammoCount = ammoCapacity;
            _canShoot = true;
        }

        private IEnumerator  StartAttackCdTimer()
        {
            yield return new WaitForSeconds(AttackCd);
            _canShoot = true;
        }
    }
}

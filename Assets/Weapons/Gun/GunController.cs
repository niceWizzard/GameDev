using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GunController : MonoBehaviour
{
    [Header("Stats")] 
    [SerializeField, Range(3, 15)] private int attackPerSecond = 4;
    [SerializeField] private int ammoCapacity = 7;
    [SerializeField, Range(4, 10)] private int accuracy = 4;
    [Header("Components")]
    [SerializeField] private Transform leftNozzleTransform;
    [SerializeField] private Transform rightNozzleTransform;
    [SerializeField] private GameObject owner;
    [SerializeField, Space(10)]
    private ProjectileController projectilePrefab;
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

    public void Shoot()
    {
        if (!_camera || !_canShoot || _isReloading)
            return;
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        _canShoot = false;
        var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = (mouse - Owner.transform.position).normalized;
        projectile.Setup(NozzleTransform.position,mouseDir, Owner ,accuracy);
        StartCoroutine(--_ammoCount <= 0 ? StartReloadTimer() : StartAttackCdTimer());
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

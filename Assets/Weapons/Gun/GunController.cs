using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GunController : MonoBehaviour
{
    [Header("Stats")] 
    [SerializeField, Range(0, 15)] private int attackPerSecond = 2;
    [SerializeField] private float attackRange = 5f;
    
    [Header("Components")]
    [SerializeField] private Transform leftNozzleTransform;
    [SerializeField] private Transform rightNozzleTransform;
    [SerializeField, Space(10)]
    private ProjectileController projectilePrefab;
    private SpriteRenderer _spriteRenderer;
    private Camera _camera;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public GameObject Owner => transform.parent.gameObject;
    private Transform NozzleTransform => _spriteRenderer.flipY ? rightNozzleTransform : leftNozzleTransform;

    private bool _canShoot = true;
    private float AttackCd => 1f / attackPerSecond;
    private void Awake()
    {
        _camera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite(bool isFlipped)
    {
        _spriteRenderer.flipY = isFlipped;
    }

    public void Shoot()
    {
        if (!_camera || !_canShoot)
            return;
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        _canShoot = false;
        var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = (mouse - Owner.transform.position).normalized;
        projectile.Setup(NozzleTransform.position,mouseDir, Owner, attackRange);
        StartCoroutine(StartAttackCdTimer());
    }

    private IEnumerator  StartAttackCdTimer()
    {
        yield return new WaitForSeconds(AttackCd);
        _canShoot = true;
    }
}

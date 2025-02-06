using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private ProjectileController projectilePrefab;
    private SpriteRenderer _spriteRenderer;
    private Camera _camera;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public GameObject Owner => transform.parent.gameObject;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite(bool isFlipped)
    {
        _spriteRenderer.flipY = isFlipped;
    }

    public void Shoot()
    {
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        if (!_camera)
            return;
        var mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = (mouse - Owner.transform.position).normalized;
        projectile.Setup(mouseDir, Owner);
    }
}

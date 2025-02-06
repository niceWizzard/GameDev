using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IDamageable
{
    public float friction = 0.5f;
    public float movementSpeed = 5f;
    public Rigidbody2D rigidbody2d;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GunController gun;
    private HealthComponent _healthComponent;
    public HealthComponent HealthComponent => _healthComponent;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnHealthZero += OnHealthZero;
    }

    private void OnHealthZero()
    {
        Destroy(gameObject);
    }

    [SerializeField] private Transform gunAnchor;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public GunController Gun => gun;
    public Transform GunAnchor => gunAnchor;
    public float FacingDirection { get; private set; } = 1;

    public void UpdateFacingDirection(Vector2 input)
    {
        if (math.abs(input.x) < 0.01f)
            return;
        FacingDirection = math.sign(input.x);
        spriteRenderer.flipX = FacingDirection < 0;
    }

}

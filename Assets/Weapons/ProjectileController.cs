#nullable enable
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxTravelDistance = 5f;
    [SerializeField] private ParticleSystem hitParticles;
    private GameObject? _projectileSender;
    private Vector2 _direction = Vector2.zero;
    
    private CircleCollider2D? _circleCollider2D ;

    private float traveledDistance = 0f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var v = _direction * (Time.fixedDeltaTime * speed);
        
        transform.position += (Vector3) v;
        traveledDistance += v.magnitude;
        if (traveledDistance >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(Vector2 pos,Vector2 dir, GameObject sender, float maxDistance)
    {
        _direction = dir.normalized;
        _projectileSender = sender;
        var angle = math.atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0,0, angle);
        transform.position = pos;
        maxTravelDistance = maxDistance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Hurtbox>()?.Hurt(
            new DamageInfo(50, _projectileSender)    
        );
        Destroy(gameObject,0.2f);
        if (_circleCollider2D)
            _circleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _direction *= 0;
        hitParticles.Play();

    }


}

#nullable enable
using Unity.Mathematics;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxTravelDistance = 5f;
    private GameObject? _projectileSender;
    private Vector2 _direction = Vector2.zero;
    
    private CircleCollider2D? _circleCollider2D ;

    private float traveledDistance = 0f;
    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
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

    public void Setup(Vector2 dir, GameObject sender)
    {
        _direction = dir.normalized;
        _projectileSender = sender;
        var angle = math.atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0,0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}

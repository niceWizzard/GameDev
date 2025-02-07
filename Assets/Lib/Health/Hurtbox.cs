using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hurtbox : MonoBehaviour, IDamageable
{
    [SerializeField]
    private HealthComponent ownerHealth;
    private Collider2D _collider;
    public event Action<DamageInfo> OnHurt;
    
    public HealthComponent HealthComponent => ownerHealth;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void Disable()
    {
        _collider.enabled = false;
    }

    public void Enable()
    {
        _collider.enabled = true;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        OnHurt?.Invoke(damageInfo);
    }

}

using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hurtbox : MonoBehaviour
{
    private Collider2D _collider;
    public event Action<DamageInfo> OnHurt;

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
    public void Hurt(DamageInfo damageInfo)
    {
        OnHurt?.Invoke(damageInfo);
    }        
}

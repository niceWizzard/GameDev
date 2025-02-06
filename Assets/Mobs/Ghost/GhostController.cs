using System;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private HealthComponent healthComponent;
    private void Awake()
    {
        hurtbox.OnHurt += HurtboxOnOnHurt;
        healthComponent.OnHealthZero += HealthComponentOnOnHealthZero;
    }

    private void HealthComponentOnOnHealthZero()
    {
        Destroy(gameObject);
    }

    private void HurtboxOnOnHurt(DamageInfo obj)
    {
        healthComponent.ReduceHealth(obj.damage);
    }
}

using System;
using Lib.Health;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class MobController : MonoBehaviour
{
    [Header("Mob Components")]
    [SerializeField] protected HealthComponent healthComponent;
    [SerializeField] protected Hurtbox hurtbox;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    
    public HealthComponent HealthComponent => healthComponent;
    public Hurtbox Hurtbox => hurtbox;
    public SpriteRenderer SpriteRenderer => spriteRenderer;


    private void Awake()
    {
        hurtbox.OnHurt += OnHurtboxHurt;
        healthComponent.OnHealthZero += OnHealthZero;
    }

    protected virtual void OnHurtboxHurt(DamageInfo damageInfo)
    {
        healthComponent.ReduceHealth(damageInfo.damage);
    }

    protected virtual void OnHealthZero()
    {
        Destroy(gameObject);
    }
    
}

using UnityEngine;

public interface IDamageable 
{
    public HealthComponent HealthComponent { get; }

    public void TakeDamage(DamageInfo damageInfo)
    {
        HealthComponent.ReduceHealth(damageInfo.damage);
    }
}

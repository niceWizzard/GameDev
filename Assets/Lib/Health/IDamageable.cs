using UnityEngine;

public interface IDamageable 
{
    public HealthComponent HealthComponent { get; }

    public void TakeDamage(float damage)
    {
        Debug.Log("DAMGED");
        HealthComponent.ReduceHealth(damage);
    }
}

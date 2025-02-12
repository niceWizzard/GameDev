namespace Main.Lib.Health
{
    public interface IDamageable 
    {
        public HealthComponent HealthComponent { get; }

        public void TakeDamage(DamageInfo damageInfo)
        {
            HealthComponent.ReduceHealth(damageInfo.damage);
        }
    }
}

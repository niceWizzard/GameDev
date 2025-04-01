using System;
using UnityEngine;

namespace Main.Lib.Health
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnHealthZero;
        public event Action<float> OnHealthChange;
        [SerializeField]
        private float startingHealth = 100f;

        private float _health = 0;

        private void Awake()
        {
            if(MaxHealth == 0)
                SetMaxHealth(startingHealth);
        }
        
        public float HealthPercentage => _health / MaxHealth;

        public float MaxHealth { get; private set; }
        public float Health => _health;

        public void Heal(float amount)
        {
            SetHealth(_health + amount);
        }

        public void ReduceHealth(float amount)
        {
            SetHealth(_health - amount);
        }

        public void SetMaxHealth(float amount)
        {
            MaxHealth = amount;
            SetHealth(MaxHealth);
        }
        
        public void SetHealth(float h)
        {
            _health = Mathf.Min(h, MaxHealth);
            OnHealthChange?.Invoke(h);
            if (_health <= 0)
            {
                OnHealthZero?.Invoke();
            }
        }
    }
}

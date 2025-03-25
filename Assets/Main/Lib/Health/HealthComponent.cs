using System;
using UnityEngine;

namespace Main.Lib.Health
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnHealthZero;
        public event Action<float> OnHealthChange;
        [SerializeField]
        private float health = 100f;
    
        public float Health => health;

        public void Heal(float amount)
        {
            SetHealth(health + amount);
        }

        public void ReduceHealth(float amount)
        {
            SetHealth(health - amount);
        }
        public void SetHealth(float h)
        {
            health = h;
            OnHealthChange?.Invoke(h);
            if (health <= 0)
            {
                OnHealthZero?.Invoke();
            }
        }
    }
}

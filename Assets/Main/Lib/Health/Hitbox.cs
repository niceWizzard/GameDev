using System;
using UnityEngine;

namespace Main.Lib.Health
{
    [RequireComponent(typeof(Collider2D))]
    public class Hitbox : MonoBehaviour
    {
        private Collider2D _collider;
        public event Action<Hurtbox> HurtboxHit; 
        
        public Collider2D Collider => _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;    
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Hurtbox hurtbox))
            {
                HurtboxHit?.Invoke(hurtbox);
            }
        }

        public void Disable()
        {
            _collider.enabled = false;
        }

        public void Enable()
        {
            _collider.enabled = true;
        }
    }
}

using System;
using Cysharp.Threading.Tasks.Triggers;
using Main.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Lib.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Item : MonoBehaviour
    {
        private enum ItemState
        {
            Disabled,
            Dropped,
            Stationary,
        }

        private Vector2 _velocity;
        private ItemState _state = ItemState.Dropped;

        private readonly float _friction = 0.8f; // Friction to slow down movement
        private readonly float _minSpeed = 0.01f; // Stop movement when very slow
        
        private SpriteRenderer _spriteRenderer;
        

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _velocity = Vector2.zero; // Default state
        }

        public void Disable()
        {
            _spriteRenderer.enabled = false;
            _state = ItemState.Disabled;
        }

        public void Enable(Vector2 position)
        {
            var angle = Random.Range(-180f, 180f);
            var dir = Quaternion.Euler(0,0,angle) * Vector2.right;
            transform.position = position;
            _velocity = dir.normalized * Random.Range(10f,15f); 
            _spriteRenderer.enabled = true;
            _state= ItemState.Dropped;
        }

        private void FixedUpdate()
        {            
            if (_state != ItemState.Dropped) return;

            // Apply friction
            _velocity *= _friction;

            // Move item
            transform.position += (Vector3)_velocity * Time.fixedDeltaTime;

            // Stop when very slow
            if (_velocity.sqrMagnitude >= _minSpeed * _minSpeed)
                return;
            _velocity = Vector2.zero;
            _state = ItemState.Stationary;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!other.TryGetComponent<PlayerController>(out var player))
                return;
            OnPickedUp(player);
        }

        protected virtual void OnPickedUp(PlayerController player)
        {
            Destroy(gameObject);
        }
    }
}
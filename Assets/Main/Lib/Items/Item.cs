using System;
using Main.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Lib.Items
{
    public class Item : MonoBehaviour
    {
        private enum ItemState
        {
            Disabled,
            Dropped,
            PickedUp,
            Stationary,
        }

        private Vector2 _velocity;
        private ItemState _state = ItemState.Dropped;

        private readonly float _friction = 0.95f; // Friction to slow down movement
        private readonly float _minSpeed = 0.01f; // Stop movement when very slow

        private void ChangeState(ItemState newState)
        {
            _state = newState;

            // On Enter
            switch (_state)
            {
                case ItemState.Disabled:
                    gameObject.SetActive(false);
                    break;
                case ItemState.Dropped:
                    gameObject.SetActive(true);
                    break;
            }
        }

        private void Awake()
        {
            _velocity = Vector2.zero; // Default state
        }

        public void Disable()
        {
            ChangeState(ItemState.Disabled);
        }

        public void Enable(Vector2 position)
        {
            var angle = Random.Range(-Mathf.PI, Mathf.PI);
            var dir = Quaternion.Euler(0,0,angle) * Vector2.right;
            transform.position = position;
            _velocity = dir.normalized * Random.Range(10f, 30f); 
            ChangeState(ItemState.Dropped);
        }

        private void Update()
        {
            if (_state != ItemState.Dropped) return;

            // Apply friction
            _velocity *= _friction;

            // Move item
            transform.position += (Vector3)_velocity * Time.deltaTime;

            // Stop when very slow
            if (_velocity.sqrMagnitude >= _minSpeed * _minSpeed)
                return;
            _velocity = Vector2.zero;
            ChangeState(ItemState.Stationary);
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
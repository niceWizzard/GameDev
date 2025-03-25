using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Lib.Items
{
    public class Item : MonoBehaviour
    {
        private float _travelDistance;

        private bool _isEnabled = false;
        private Vector3 _direction;

        private bool _reachedEnd = false;

        private void Awake()
        {
            _travelDistance = Random.Range(1, 3);
        }

        public void Disable()
        {
            _isEnabled = false;
            gameObject.SetActive(false);
        }

        public void Enable(Vector3 dir, Vector2 position)
        {
            gameObject.SetActive(true);
            _direction = dir;
            transform.parent = null;
            transform.position = position;
            _isEnabled = true;
        }

        private void Update()
        {
            if (!_isEnabled || _reachedEnd)
                return;
            var vel = _direction * (Time.deltaTime * 5.5f);
            transform.position += vel;
            _travelDistance -= vel.magnitude;
            if (_travelDistance < 0)
            {
                _reachedEnd = true;
            }
        }
    }
}

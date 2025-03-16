using System;
using Main.Weapons.Bullet;
using UnityEngine;

namespace Main.World.Objects.Pedestal
{
    public class PedestalController : MonoBehaviour
    {
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private bool oneShot = true;
        private bool _hasActivated = false;
        public event Action Activated;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet")) return;
            if (oneShot && _hasActivated)
                return;
            Activate();
        }

        private void Activate()
        {
            GetComponent<SpriteRenderer>().sprite = activeSprite;
            _hasActivated = true;
            Activated?.Invoke();
        }
    }
}

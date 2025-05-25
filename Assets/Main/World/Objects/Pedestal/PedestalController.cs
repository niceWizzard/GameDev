using System;
using Main.Weapons.Bullet;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Main.World.Objects.Pedestal
{
    public class PedestalController : MonoBehaviour
    {
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private bool oneShot = true;
        [SerializeField] private bool requiredInLevel = false;
        private Light2D light;
        private bool _hasActivated = false;
        
        public bool IsActive { get; private set; }
        public event Action Activated;

        private void Awake()
        {
            light = transform.GetComponentInChildren<Light2D>();
        }

        private void Start()
        {
            if(requiredInLevel)
                GameManager.CurrentLevel.Register(this);
        }

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
            IsActive = true;
            light.pointLightOuterRadius += 4;
            light.pointLightInnerRadius += 2;
            Activated?.Invoke();
        }
    }
}

using System;
using Main.Player;
using Main.Weapons.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI
{
    public class HealthHUDController : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        private PlayerController _player;

        private void Awake()
        {
            Disable();
        }

        public void Disable()
        {
            healthBar.gameObject.SetActive(false);
        }

        public void Enable()
        {
            healthBar.gameObject.SetActive(true);
        }

        public void Setup(PlayerController p)
        {
            if (!p)
                return;
            Enable();
            _player = p;
            _player.HealthComponent.OnHealthChange += health => healthBar.value = health;
            PlayerController.StatChange += PlayerControllerOnStatChange;
        }

        private void PlayerControllerOnStatChange(RangedStats obj)
        {
            healthBar.maxValue = obj.Health;
            healthBar.value = _player.HealthComponent.Health;
        }
    }
}

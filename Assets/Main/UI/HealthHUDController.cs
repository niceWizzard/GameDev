using Main.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI
{
    public class HealthHUDController : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        private PlayerController _player;

        public void Setup(PlayerController p)
        {
            if (!p)
                return;
            _player = p;
            healthBar.maxValue = _player.HealthComponent.Health;
            healthBar.value = _player.HealthComponent.Health;
            _player.HealthComponent.OnHealthChange += health => healthBar.value = health; 
        }
    }
}

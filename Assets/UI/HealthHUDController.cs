using Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class HealthHUDController : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        public PlayerController Player { get; set; }

        private void Start()
        {
            healthBar.maxValue = Player.HealthComponent.Health;
            healthBar.value = Player.HealthComponent.Health;
            Player.HealthComponent.OnHealthChange += health => healthBar.value = health; 
        }
    }
}

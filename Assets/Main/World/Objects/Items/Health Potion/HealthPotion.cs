using Main.Lib.Items;
using Main.Player;
using UnityEngine;

namespace Main.World.Objects.Items.Health_Potion
{
    public class HealthPotion : Item
    {
        [SerializeField] private float healAmount = 25;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!other.TryGetComponent<PlayerController>(out var player))
                return;
            player.HealthComponent.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}

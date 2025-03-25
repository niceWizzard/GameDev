using Main.Lib.Items;
using Main.Player;
using UnityEngine;

namespace Main.World.Objects.Items.Health_Potion
{
    public class HealthPotion : Item
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!other.TryGetComponent<PlayerController>(out var player))
                return;
            Debug.Log("HEALTH POTIION!");
            Destroy(gameObject);
        }
    }
}

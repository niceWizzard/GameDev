using Main.Lib.Items;
using Main.Player;
using UnityEngine;

namespace Main.World.Objects.Items.Health_Potion
{
    public class HealthPotion : Item
    {
        [SerializeField] private float healAmount = 25;
        protected override void OnPickedUp(PlayerController player)
        {
            base.OnPickedUp(player);
            player.HealthComponent.Heal(healAmount);
        }
    }
}

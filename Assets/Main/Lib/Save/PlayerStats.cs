using UnityEngine;

namespace Main.Lib.Save
{
    public record PlayerStats
    {
        public float Health { get; set; } = 100f;
        public float AttackPower { get; set; } = 20f;
        public int AttackPerSecond { get; set; } = 4;
        public int AmmoCapacity { get; set; } = 15;
        public int Accuracy { get; set; } = 4;
        public float ReloadTime { get; set; } = 2f;
        public float MovementSpeed { get; set; } = 5f;
        
    }
}

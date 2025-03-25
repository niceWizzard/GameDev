using Main.Lib.Stat;
using UnityEngine;

namespace Main.Weapons.Gun
{
    public class GunnerStats : Stats
    {
        [Header("Gunner")] [SerializeField] private float specialAttackMultiplier = 1.5f;
        [SerializeField] private int ammoCapacity = 7;
        [SerializeField, Range(3, 15)] private int attackPerSecond = 4;
        [SerializeField, Range(4, 10)] private int accuracy = 4;
        [SerializeField, Range(0.5f, 5f)] private float reloadTime = 2f;

        public float SpecialAttackMultiplier => specialAttackMultiplier;
        public int AmmoCapacity => ammoCapacity;
        public int AttackPerSecond => attackPerSecond;
        public int Accuracy => accuracy;
        public float SpecialAttackDamage => AttackPower * specialAttackMultiplier;
        public float ReloadTime => reloadTime;
    }
}

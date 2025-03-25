using System.Collections.Generic;
using Main.Lib.Save;
using Main.Lib.Stat;
using UnityEngine;
using StatUpgrade = Main.Lib.Save.Stats.StatUpgrade;

namespace Main.Weapons.Gun
{
    public class RangedStats : Stats
    {
        [Header("Ranged")] [SerializeField] private float specialAttackMultiplier = 1.5f;
        [SerializeField] private int ammoCapacity = 7;
        [SerializeField, Range(3, 15)] private int attackPerSecond = 4;
        [SerializeField, Range(4, 10)] private int accuracy = 4;
        [SerializeField, Range(0.5f, 5f)] private float reloadTime = 2f;
        [SerializeField, Range(5, 25)] private float projectileSpeed = 5f; 
        [SerializeField] private bool disposeProjectilesOnDeath = false;

        public float SpecialAttackMultiplier => specialAttackMultiplier;
        public int AmmoCapacity => ammoCapacity;
        public int AttackPerSecond => attackPerSecond;
        public int Accuracy => accuracy;
        public float SpecialAttackDamage => AttackPower * specialAttackMultiplier;
        public float ReloadTime => reloadTime;
        public float ProjectileSpeed => projectileSpeed;
        public bool DisposeProjectilesOnDeath => disposeProjectilesOnDeath;

        public void SetFromSave(IReadOnlyList<string> upgrades)
        {
            foreach (var upgrade in upgrades)
            {
                switch (upgrade)
                {
                    case StatUpgrade.Accuracy:
                        accuracy += 1;
                        break;
                    case StatUpgrade.ReloadTime:
                        reloadTime *= 0.9f;
                        break;
                    case StatUpgrade.AmmoCapacity:
                        ammoCapacity += 5;
                        break;
                    case StatUpgrade.AttackPerSecond:
                        attackPerSecond += 1;
                        break;
                    case StatUpgrade.Health:
                        health += 25;
                        break;
                    case StatUpgrade.Speed:
                        movementSpeed += .5f;
                        break;
                    case StatUpgrade.AttackPower:
                        attackPower *= 1.25f;
                        break;
                    default:
                        Debug.LogError($"Unknown stat upgrade: {upgrade}");
                        break;
                }
            }
        }
    }
}

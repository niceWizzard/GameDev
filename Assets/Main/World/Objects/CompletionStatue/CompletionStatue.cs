using System;
using System.Linq;
using Main.Lib;
using Main.Lib.Save;
using Main.Lib.Save.Stats;
using Main.Lib.Singleton;
using UnityEngine;

namespace Main.World.Objects.CompletionStatue
{
    public enum StatType
    {
        Health,
        Attack,
        Speed,
        Reload,
        Ammo,
        Accuracy
    }
    [RequireComponent(typeof(SpriteRenderer), typeof(Interactable))]
    public class CompletionStatue : MonoBehaviour
    {
        [SerializeField] private StatType leftRewardType = StatType.Health;
        [SerializeField] private StatType rightRewardType = StatType.Ammo;

        [SerializeField] private Interactable leftRewardInteractable;
        [SerializeField] private Interactable rightRewardInteractable;

        private Interactable _statueInteractable;

        private void Start()
        {
            _statueInteractable = GetComponent<Interactable>();
            _statueInteractable.OnInteract += StatueInteractableOnInteract;
            leftRewardInteractable.OnInteract += LeftRewardInteractableOnInteract;
            rightRewardInteractable.OnInteract += RightRewardInteractableOnInteract;
            
            _statueInteractable.IsInteractable = false;
            
            leftRewardInteractable.SetText(
                GetStatText(leftRewardType)
            );
            rightRewardInteractable.SetText(
                GetStatText(rightRewardType)
            );
        }

        private void OnDestroy()
        {
            leftRewardInteractable.OnInteract -= LeftRewardInteractableOnInteract;
            rightRewardInteractable.OnInteract -= RightRewardInteractableOnInteract;
            _statueInteractable.OnInteract -= StatueInteractableOnInteract;
        }

        private void RightRewardInteractableOnInteract()
        {
            leftRewardInteractable.IsInteractable = false;
            rightRewardInteractable.IsInteractable = false;
            GiveReward(rightRewardType);
        }

        private void LeftRewardInteractableOnInteract()
        {
            leftRewardInteractable.IsInteractable = false;
            rightRewardInteractable.IsInteractable = false;
            GiveReward(leftRewardType);
        }

        private void GiveReward(StatType statType)
        {
            _statueInteractable.IsInteractable = true;
            var upgrade = statType switch
            {
                StatType.Health => StatUpgrade.Health,
                StatType.Attack => StatUpgrade.AttackPower,
                StatType.Speed => StatUpgrade.AttackPerSecond,
                StatType.Reload => StatUpgrade.ReloadTime,
                StatType.Ammo => StatUpgrade.AmmoCapacity,
                StatType.Accuracy => StatUpgrade.Accuracy,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
            Debug.Log($"Upgrading {statType}");
            // SaveManager.Instance.SaveData(v => v with
            // {
            //     StatUpgrades = v.StatUpgrades.Append(upgrade).ToList()
            // });
        }

        private static string GetStatText(StatType statType) => statType switch
        {
            StatType.Health => "Health",
            StatType.Accuracy => "Accuracy",
            StatType.Ammo => "Ammo",
            StatType.Reload => "Reload",
            StatType.Attack => "Attack",
            StatType.Speed => "Speed",
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
        
        private void StatueInteractableOnInteract()
        {
            LevelLoader.Instance.LoadLevel("HubLevel");
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Save;
using Main.Lib.Save.Stats;
using Main.Lib.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.World.Objects.CompletionStatue
{
    public enum StatType
    {
        Health,
        Attack,
        Speed,
        Reload,
        Ammo,
        Accuracy,
        Movement,
    }
    [RequireComponent( typeof(Interactable), typeof(UniqueId))]
    [RequireComponent(typeof(Animator))]
    public class CompletionStatue : MonoBehaviour
    {
        private static readonly int IsActivated = Animator.StringToHash("isActivated");
        [SerializeField] private StatType leftRewardType = StatType.Health;
        [SerializeField] private StatType rightRewardType = StatType.Ammo;

        [SerializeField] private Interactable leftRewardInteractable;
        [SerializeField] private Interactable rightRewardInteractable;
        [SerializeField] private SpriteRenderer statueSr;
        
        
        private Interactable _statueInteractable;
        private bool _isEnabled;
        private UniqueId _uniqueId;
        private bool _inSaveFile;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _uniqueId = GetComponent<UniqueId>();
            if(!_uniqueId)
                Debug.LogError($"No unique id found at {name} in {SceneManager.GetActiveScene().name}");
        }

        private void Start()
        {
            _statueInteractable = GetComponent<Interactable>();
            _statueInteractable.OnInteract += StatueInteractableOnInteract;
            leftRewardInteractable.OnInteract += LeftRewardInteractableOnInteract;
            rightRewardInteractable.OnInteract += RightRewardInteractableOnInteract;
            _inSaveFile = SaveManager.Instance.SaveGameData.CompletedLevels.Contains(SceneManager.GetActiveScene().name);
            
            _statueInteractable.IsInteractable = false;
            leftRewardInteractable.IsInteractable= false;
            rightRewardInteractable.IsInteractable = false;

            if (_inSaveFile) return;
            leftRewardInteractable.SetText(
                GetStatText(leftRewardType)
            );
            rightRewardInteractable.SetText(
                GetStatText(rightRewardType)
            );

        }

        public void Setup()
        {
            _isEnabled = true;
            _animator.SetBool(IsActivated, true);
            statueSr.transform.DOShakePosition(0.5f, strength: 0.05f, vibrato: 10, randomness: 90).SetLink(gameObject).SetLoops(-1, LoopType.Incremental);
            if (_inSaveFile)
            {
                _statueInteractable.IsInteractable = true;
                _statueInteractable.SetText("Return");
            }
            else
            {
                leftRewardInteractable.IsInteractable = true;
                rightRewardInteractable.IsInteractable = true;
            }
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
            _animator.SetBool(IsActivated, true);
            var upgrade = statType switch
            {
                StatType.Health => StatUpgrade.Health,
                StatType.Attack => StatUpgrade.AttackPower,
                StatType.Speed => StatUpgrade.AttackPerSecond,
                StatType.Reload => StatUpgrade.ReloadTime,
                StatType.Ammo => StatUpgrade.AmmoCapacity,
                StatType.Accuracy => StatUpgrade.Accuracy,
                StatType.Movement => StatUpgrade.Speed,
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
            var sceneName = SceneManager.GetActiveScene().name;
            _ = SaveManager.Instance.SaveDataAsync(v => v with
            {
                StatUpgrades = v.StatUpgrades.Append(upgrade).ToList(),
                BrokenStatues = v.BrokenStatues.Append(_uniqueId.Id).ToHashSet(),
                CompletedLevels = v.CompletedLevels.Append(sceneName).ToHashSet(),
            });
            var message = statType switch
            {
                StatType.Health => "Health increased by 25",
                StatType.Attack => "Attack Power + 10%",
                StatType.Speed => "Attack Speed increased",
                StatType.Reload => "Reload Time decreased by 0.25s",
                StatType.Ammo => "Ammo Increased by 5",
                StatType.Accuracy => "Accuracy Increased",
                StatType.Movement => "Movement Speed increased",
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
            _ = Dialog.CreateDialog(message);
        }

        private static string GetStatText(StatType statType) => statType switch
        {
            StatType.Health => "Health",
            StatType.Accuracy => "Accuracy",
            StatType.Ammo => "Ammo",
            StatType.Reload => "Reload",
            StatType.Attack => "Attack",
            StatType.Speed => "Speed",
            StatType.Movement => "Movement",
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
        
        private void StatueInteractableOnInteract()
        {
            LevelLoader.Instance.LoadHub();
        }

        
    }
}

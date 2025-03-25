using System;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Save;
using Main.Lib.Save.Stats;
using UnityEngine;

namespace Main.World.Objects.Chest
{
    [RequireComponent(typeof(UniqueId), typeof(Interactable), typeof(SpriteRenderer))]
    public class StatChest : MonoBehaviour
    {
        [SerializeField] private Sprite openedSprite;
        [SerializeField] private GameObject uiPopup;
        private UniqueId _uniqueId;
        private Interactable _interactable;

        private bool _isOpen;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _uniqueId = GetComponent<UniqueId>();
            _interactable = GetComponent<Interactable>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _interactable.OnInteract += InteractableOnOnInteract;
        }

        private void Start()
        {
            uiPopup.SetActive(false);
        }

        private void OnDestroy()
        {
            _interactable.OnInteract -= InteractableOnOnInteract;
        }

        private void InteractableOnOnInteract()
        {
            _isOpen = !_isOpen;
            if (!_isOpen) return;
            Time.timeScale = 0;
            uiPopup.transform.localScale *= 0;
            uiPopup.SetActive(true);
            uiPopup.transform.DOScale(Vector3.one, 0.1f).SetUpdate(true).SetLink(gameObject);
            _interactable.IsInteractable = false;
            _spriteRenderer.sprite = openedSprite;
        }

        public void StatButtonClick(string buttonName)
        {
            var save = SaveManager.Instance;
            var upgrade = buttonName.ToLower() switch
            {
                "ap"       => StatUpgrade.AttackPower,
                "health"   => StatUpgrade.Health,
                "movement" => StatUpgrade.Speed,
                "ammo"     => StatUpgrade.AmmoCapacity,
                "dps"      => StatUpgrade.AttackPerSecond,
                "reload"   => StatUpgrade.ReloadTime,
                "accuracy" => StatUpgrade.Accuracy,
                _ => throw new ArgumentOutOfRangeException(nameof(buttonName), buttonName, null)
            };
            save.SaveData(v => v with
            {
                StatUpgrades = v.StatUpgrades.Append(upgrade).ToList()
            });
            Time.timeScale = 1;
            uiPopup.SetActive(false);
        }
    }
}

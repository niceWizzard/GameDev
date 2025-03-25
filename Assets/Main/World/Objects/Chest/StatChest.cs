using System;
using CleverCrow.Fluid.UniqueIds;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Save;
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
            switch (buttonName.ToLower())
            {
                case "ap":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            AttackPower = v.PlayerStats.AttackPower + 10
                        }
                    });
                    break;
        
                case "health":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            Health = v.PlayerStats.Health + 20
                        }
                    });
                    break;
        
                case "movement":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            MovementSpeed = v.PlayerStats.MovementSpeed + 0.5f
                        }
                    });
                    break;
        
                case "ammo":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            AmmoCapacity = v.PlayerStats.AmmoCapacity + 5
                        }
                    });
                    break;
        
                case "dps":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            AttackPerSecond = v.PlayerStats.AttackPerSecond + 1
                        }
                    });
                    break;
        
                case "reload":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            ReloadTime = v.PlayerStats.ReloadTime * 0.9f // 10% faster reload
                        }
                    });
                    break;
        
                case "accuracy":
                    save.SaveData(v => v with
                    {
                        PlayerStats = v.PlayerStats with
                        {
                            Accuracy = v.PlayerStats.Accuracy + 5
                        }
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttonName), buttonName, null);
            }

            Time.timeScale = 1;
            uiPopup.SetActive(false);
        }
    }
}

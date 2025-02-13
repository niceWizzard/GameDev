using System;
using DG.Tweening;
using Main.Lib;
using TMPro;
using UnityEngine;

namespace Main.World.Objects.Lamp
{
    public class LampController : MonoBehaviour, IInteractable
    {
        private static readonly int Size = Shader.PropertyToID("_size");
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite activatedSprite;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private TMP_Text text;
        
        
        
        
        private bool IsActive { get; set; }
        
        private  Material _interactableMaterial;

        private void Start()
        {
            _interactableMaterial = new Material(spriteRenderer.sharedMaterial);
            var color = text.color;
            color.a = 0f;
            text.color = color;
            spriteRenderer.material = _interactableMaterial;
        }

        private void Toggle()
        {
            IsActive = !IsActive;
            spriteRenderer.sprite = IsActive ? activatedSprite : inactiveSprite;
        }

        public bool IsUiShown { get; private set; } = false;
        public Transform Transform => transform;
        public event Action OnInteract;

        public void Interact()
        {
            Toggle();
        }

        public void ShowUI()
        {
            IsUiShown = true;
            _interactableMaterial.DOFloat(IInteractable.OutlineSize, Size, IInteractable.TransitionDuration);
            text.DOFade(1, IInteractable.TransitionDuration).SetEase(Ease.OutBounce);
        }

        public void HideUI()
        {
            IsUiShown = false;
            text.DOFade(0, IInteractable.TransitionDuration).SetEase(Ease.OutBounce);
            _interactableMaterial.DOFloat(0f, Size, IInteractable.TransitionDuration);

        }
    }
}

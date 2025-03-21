using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.Lib
{
    public class Interactable: MonoBehaviour
    {
        private static readonly int Size = Shader.PropertyToID("_size");
        public const float InteractableDistance = 1f;
        private const float OutlineSize = 0.75f;
        private const float TransitionDuration = 0.15f;
            
        [SerializeField] private bool isInteractable = true;
        [SerializeField] private string textShown = "Interact";
        [SerializeField] private TMP_Text textUi;

        public bool IsUiShown { get; private set; } = false;

        public bool IsInteractable
        {
            get => isInteractable;
            set
            {
                isInteractable = value;
                if(!value)
                    HideUI();
            }
        }

        public event Action OnInteract;

        public void Interact()
        {
            if (isInteractable)
                OnInteract?.Invoke();
        }
        
        private  Material _interactableMaterial;
        private SpriteRenderer _spriteRenderer;


        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _interactableMaterial = new Material(_spriteRenderer.sharedMaterial);
            _spriteRenderer.material = _interactableMaterial;
            
            textUi.text = $"{textShown} (E)";
            var color = textUi.color;
            color.a = 0f;
            textUi.color = color;
        }

        public void SetText(string text)
        {
            textUi.text = text;
        }

        public void ShowUI()
        {
            IsUiShown = true;
            _interactableMaterial.DOFloat(OutlineSize, Size, TransitionDuration);
            textUi.DOFade(1, TransitionDuration).SetEase(Ease.OutBounce);
        }

        public void HideUI()
        {
            IsUiShown = false;
            textUi.DOFade(0, TransitionDuration).SetEase(Ease.OutBounce);
            _interactableMaterial.DOFloat(0f, Size, TransitionDuration);
        }
    
    }
}

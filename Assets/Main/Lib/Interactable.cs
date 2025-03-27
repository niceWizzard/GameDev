using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.Lib
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable: MonoBehaviour
    {
        private static readonly int Size = Shader.PropertyToID("_size");
        public const float InteractableDistance = 1.5f;
        private const float OutlineSize = 0.75f;
        private const float TransitionDuration = 0.15f;
        
        [SerializeField] private List<SpriteRenderer> spriteRenderers;
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
            if (isInteractable && Mathf.Approximately(Time.timeScale, 1))
                OnInteract?.Invoke();
        }
        
        private  Material _interactableMaterial;


        protected virtual void Awake()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if(spriteRenderer)
                spriteRenderers.Add(spriteRenderer);
            
            _interactableMaterial = new Material(spriteRenderers[0].sharedMaterial);
            foreach (var spriteRenderer1 in spriteRenderers)
            {
                spriteRenderer1.material = _interactableMaterial;
            }
            
            
            textUi.text = $"{textShown} (E)";
            var color = textUi.color;
            color.a = 0f;
            textUi.color = color;
        }

        public void SetText(string text)
        {
            textUi.text = $"{text} (E)";
        }

        public void ShowUI()
        {
            IsUiShown = true;
            _interactableMaterial.DOFloat(OutlineSize, Size, TransitionDuration).SetLink(gameObject);
            textUi.DOFade(1, TransitionDuration).SetEase(Ease.OutBounce).SetLink(gameObject);
        }

        public void HideUI()
        {
            IsUiShown = false;
            textUi.DOFade(0, TransitionDuration).SetEase(Ease.OutBounce).SetLink(gameObject);
            _interactableMaterial.DOFloat(0f, Size, TransitionDuration).SetLink(gameObject);
        }
    
    }
}

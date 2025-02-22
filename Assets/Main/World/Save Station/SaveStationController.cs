using System;
using CleverCrow.Fluid.UniqueIds;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Save;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.World.Save_Station
{
    [RequireComponent(typeof(UniqueId))]
    public class SaveStationController : MonoBehaviour, IInteractable
    {
        private static readonly int Size = Shader.PropertyToID("_size");
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text text;
        private Material _interactableMaterial;
        public bool IsUiShown { get; private set; }
        public Transform Transform => transform;
        public event Action OnInteract;
        
        private UniqueId _uniqueId;

        private void Start()
        {
            _uniqueId = GetComponent<UniqueId>();
            _interactableMaterial = new Material(spriteRenderer.sharedMaterial);
            var textColor = text.color;
            textColor.a = 0;
            text.color = textColor;
            spriteRenderer.material = _interactableMaterial;
        }

        public void Interact()
        {
            OnInteract?.Invoke();
            _ = SaveManager.Instance.SaveDataAsync(data => data with
            {
                LastSaveStation = new SaveStation()
                {
                    levelName = SceneManager.GetActiveScene().name,
                    stationId = _uniqueId.Id,
                } 
            });
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

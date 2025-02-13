using System;
using UnityEngine;

namespace Main.Lib
{
    public interface IInteractable
    {
        public const float InteractableDistance = 1f;
        public bool IsUiShown { get; }
        public Transform Transform { get; }
        public event Action OnInteract;
        public void Interact();
        
        public void ShowUI();
        public void HideUI();
    
    }
}

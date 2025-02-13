#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Main.Lib;
using UnityEngine;

namespace Main.Player
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractableDetector : MonoBehaviour
    {
        private readonly List<IInteractable> _interactables = new();
        
        private IInteractable? _currentInteractable;

        private IInteractable? CurrentInteractable
        {
            get => _currentInteractable;
            set
            {
                if (_currentInteractable is { IsUiShown: true })
                {
                    _currentInteractable?.HideUI();
                }
                _currentInteractable = value;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                _interactables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent<IInteractable>(out var interactable)) return;
            interactable.HideUI();
            _interactables.Remove(interactable);
        }

        private void Update()
        {
            if (_interactables.Count == 0)
                return;
            CurrentInteractable = _interactables.OrderBy(v => (transform.position - v.Transform.position).sqrMagnitude)
                .First();
            Vector2 toClosest = CurrentInteractable.Transform.position - transform.position;
            switch (toClosest.magnitude)
            {
                case <= IInteractable.InteractableDistance:
                    if(!CurrentInteractable.IsUiShown) 
                        CurrentInteractable.ShowUI();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        CurrentInteractable.Interact();
                    }
                    break;
                case > IInteractable.InteractableDistance when CurrentInteractable.IsUiShown:
                    CurrentInteractable.HideUI();
                    return;
                
            }
        }
    }
}

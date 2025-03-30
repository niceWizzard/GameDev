#nullable enable
using System.Collections.Generic;
using System.Linq;
using Main.Lib;
using UnityEngine;

namespace Main.Player
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractableDetector : MonoBehaviour
    {
        private readonly List<Interactable> _interactables = new();
        
        private Interactable? _currentInteractable;

        private Interactable? CurrentInteractable
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
            if (other.TryGetComponent<Interactable>(out var interactable) )
            {
                _interactables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent<Interactable>(out var interactable)) return;
            interactable.HideUI();
            _interactables.Remove(interactable);
        }

        private void Update()
        {
            if (_interactables.Count == 0 || Time.timeScale == 0)
                return;
            if (CurrentInteractable && !CurrentInteractable.IsInteractable )
            {
                if(CurrentInteractable.IsUiShown)
                    CurrentInteractable.HideUI();
            } 
            var filtered = _interactables.Where(v => v.IsInteractable).ToList();
            if (!filtered.Any())
                return;
            var closest = filtered.OrderBy(v => (transform.position - v.transform.position).sqrMagnitude)
                .First();
            if (!closest)
                return;
            CurrentInteractable = closest;
            
            Vector2 toInteractable = CurrentInteractable.transform.position - transform.position;
            switch (toInteractable.magnitude)
            {
                case <= Interactable.InteractableDistance:
                    if(!CurrentInteractable.IsUiShown) 
                        CurrentInteractable.ShowUI();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        CurrentInteractable.Interact();
                    }
                    break;
                case > Interactable.InteractableDistance when CurrentInteractable.IsUiShown:
                    CurrentInteractable.HideUI();
                    return;
                
            }
        }
    }
}

using System;
using DG.Tweening;
using Main.Lib;
using Main.World.Objects.Pedestal;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Objects.Door
{
    [RequireComponent(typeof(Collider2D), typeof(Interactable))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftDoor;
        [SerializeField] private SpriteRenderer rightDoor;
        private Collider2D _collider;
        private Interactable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
            VerifyRequirements();
            _collider = GetComponent<Collider2D>();
            
            _interactable.OnInteract += Open;
        }

        private void OnDestroy()
        {
            _interactable.OnInteract -= Open;
        }

        private void VerifyRequirements()
        {
            if(!leftDoor)
                Debug.LogError($"Left door is not set at {name}");
            if(!rightDoor)
                Debug.LogError($"Right door is not set at {name}");
            if(!_interactable)
                Debug.LogError($"Interactable is not set at {name}");
        }

        private void Open()
        {
            _interactable.IsInteractable = false;
            leftDoor.transform.DOMoveX(transform.position.x - 1, 0.8f).SetLink(gameObject);
            rightDoor.transform.DOMoveX(transform.position.x + 1, 0.8f).SetLink(gameObject);
            leftDoor.DOFade(0, 0.2f).SetLink(gameObject);
            rightDoor.DOFade(0, 0.2f).SetLink(gameObject);
            _collider.enabled = false;
        }
    }
}

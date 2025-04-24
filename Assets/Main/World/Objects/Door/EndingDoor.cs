using System;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Save;
using UnityEngine;

namespace Main.World.Objects.Door
{
    public class EndingDoor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftDoor;
        [SerializeField] private SpriteRenderer rightDoor;
        private Interactable _interactable;
        private Collider2D _collider;
        private bool _isBossDefeated;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
            _collider = GetComponent<Collider2D>();
            _interactable.OnInteract += Open;
        }

        private void Start()
        {
            _isBossDefeated = SaveManager.Instance.SaveGameData.CompletedLevels.Contains("Boss");
            var text = _isBossDefeated ? "Unlock" : "Destroy the Seals First.";
            _interactable.SetText(text);
        }

        private void OnDestroy()
        {
            _interactable.OnInteract -= Open;
        }

        private void Open()
        {
            if (!_isBossDefeated)
                return;
            _interactable.IsInteractable = false;
            leftDoor.transform.DOMoveX(transform.position.x - 1, 0.8f).SetLink(gameObject);
            rightDoor.transform.DOMoveX(transform.position.x + 1, 0.8f).SetLink(gameObject);
            leftDoor.DOFade(0, 0.2f).SetLink(gameObject);
            rightDoor.DOFade(0, 0.2f).SetLink(gameObject);
            _collider.enabled = false;
        }

        
    }
}

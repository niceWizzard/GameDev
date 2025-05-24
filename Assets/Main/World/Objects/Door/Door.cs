using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Main.Lib;
using Main.World.Objects.Pedestal;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Objects.Door
{
    [RequireComponent(typeof(Collider2D), typeof(Interactable), typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftDoor;
        [SerializeField] private SpriteRenderer rightDoor;
        [SerializeField] private List<KeyItem> requiredKeys = new();
        [SerializeField]
        private AudioSource lockedDoorSfx;
        
        [SerializeField]
        private AudioSource doorOpenSfx;
        
        private List<string> _keyIds = new();
        private Collider2D _collider;
        private Interactable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
            lockedDoorSfx = GetComponent<AudioSource>();
            VerifyRequirements();
            _collider = GetComponent<Collider2D>();
            _interactable.OnInteract += Open;
        }

        private void Start()
        {
            if (requiredKeys.Count > 0)
            {
                _keyIds = requiredKeys.Select(
                    v => v.UniqueId.Id
                ).ToList() ;
            }
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
            if(!lockedDoorSfx)
                Debug.LogError($"Locked door is not set at {name}");
        }

        public int KeysCollected() => _keyIds.Count(v => GameManager.CurrentLevel.CollectedKeys.Contains(v));
        public bool IsKeysCollected() => KeysCollected() == _keyIds.Count;
        private void FixedUpdate()
        {
            if (_keyIds.Count == 0)
                return;
            var text = KeysCollected() == _keyIds.Count ? $"Open ({_keyIds.Count}/{_keyIds.Count} Keys)" : $"Locked ({KeysCollected()}/{_keyIds.Count} Keys)";
            _interactable.SetText(text);
        }

        private void Open()
        {
            var level = GameManager.CurrentLevel;
            if (_keyIds.Count > 0 && !IsKeysCollected())
            {
                if (lockedDoorSfx.isPlaying) 
                    return;
                leftDoor.transform.DOShakePosition(0.5f, strength: new Vector3(0.03f, 0f, 0f), vibrato: 10).SetLink(gameObject);
                rightDoor.transform.DOShakePosition(0.5f, strength: new Vector3(0.03f, 0f, 0f), vibrato: 10).SetLink(gameObject);
                lockedDoorSfx.Play();
                return;
            }
            _interactable.IsInteractable = false;
            doorOpenSfx.Play();
            leftDoor.transform.DOMoveX(transform.position.x - 1, 0.8f).SetLink(gameObject);
            rightDoor.transform.DOMoveX(transform.position.x + 1, 0.8f).SetLink(gameObject);
            leftDoor.DOFade(0, 0.2f).SetLink(gameObject);
            rightDoor.DOFade(0, 0.2f).SetLink(gameObject);
            _collider.enabled = false;
        }
    }
}

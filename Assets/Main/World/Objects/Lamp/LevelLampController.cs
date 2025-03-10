using System;
using System.Linq;
using Main.Lib;
using Main.Lib.Save;
using Main.Lib.Singleton;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main.World.Objects.Lamp
{
    [RequireComponent(typeof(Interactable))]
    public class LevelLampController : MonoBehaviour
    {
        [SerializeField] private Sprite activatedSprite;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private AssetReference levelReference; 
        private SpriteRenderer _spriteRenderer;

        private bool IsActive { get; set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var levelId = levelReference.AssetGUID;
            if (SaveManager.Instance.SaveGameData.CompletedLevels.Contains(levelId))
            {
                _spriteRenderer.sprite = activatedSprite;
            }
        }

        private void Start()
        {
            var i = GetComponent<Interactable>();
            if (levelReference == null || string.IsNullOrWhiteSpace(levelReference.AssetGUID))
                throw new ArgumentNullException($"Level Ref is not set in {name}");

            i.OnInteract += Toggle;
        }


        private void Toggle()
        {
            LevelLoader.Instance.LoadLevel(levelReference);
        }
        
        


    }
}

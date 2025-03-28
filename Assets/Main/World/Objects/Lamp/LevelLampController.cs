using Main.Lib;
using Main.Lib.Save;
using Main.Lib.Singleton;
using UnityEngine;

namespace Main.World.Objects.Lamp
{
    [RequireComponent(typeof(Interactable))]
    public class LevelLampController : MonoBehaviour
    {
        [SerializeField] private Sprite activatedSprite;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private string levelName = "HubLevel";
        [SerializeField] private LevelLampController activateFirst;
        
        private SpriteRenderer _spriteRenderer;

        private bool IsActive { get; set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (activateFirst && activateFirst == this)
            {
                Debug.LogError($"You have required itself to be active first at {name}");
            }
            if (!SaveManager.Instance.SaveGameData.CompletedLevels.Contains(levelName)) return;
            _spriteRenderer.sprite = activatedSprite;
            IsActive = true;
        }

        private void Start()
        {
            var i = GetComponent<Interactable>();
            i.OnInteract += Toggle;
            if (activateFirst && !activateFirst.IsActive)
            {
                i.IsInteractable = false;
            }
        }


        private void Toggle()
        {
            LevelLoader.Instance.LoadLevel(levelName);
        }
        
        


    }
}

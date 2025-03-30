using Main.Lib;
using Main.Lib.Save;
using Main.Lib.Singleton;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Main.World.Objects.Lamp
{
    [RequireComponent(typeof(Interactable))]
    public class SealLampController : MonoBehaviour
    {
        [SerializeField] private string levelName = "HubLevel";
        [SerializeField] private SealLampController activateFirst;
        
        private SpriteRenderer _spriteRenderer;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (activateFirst && activateFirst == this)
            {
                Debug.LogError($"You have required itself to be active first at {name}");
            }

            var hasBeenCleared = SaveManager.Instance.SaveGameData.CompletedLevels.Contains(levelName);
            IsActive = hasBeenCleared;
            
        }

        private void Start()
        {
            var i = GetComponent<Interactable>();
            i.OnInteract += Toggle;
            i.IsInteractable = !IsActive;
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

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
        private SpriteRenderer _spriteRenderer;

        private bool IsActive { get; set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (SaveManager.Instance.SaveGameData.CompletedLevels.Contains(levelName))
            {
                _spriteRenderer.sprite = activatedSprite;
            }
        }

        private void Start()
        {
            var i = GetComponent<Interactable>();
            i.OnInteract += Toggle;
        }


        private void Toggle()
        {
            LevelLoader.Instance.LoadLevel(levelName);
        }
        
        


    }
}

using Main.Lib;
using Main.Lib.Singleton;
using UnityEngine;

namespace Main.World.Objects.Lamp
{
    [RequireComponent(typeof(Interactable))]
    public class LevelLampController : MonoBehaviour
    {
        [SerializeField] private Sprite activatedSprite;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private string levelName; 
        private SpriteRenderer _spriteRenderer;

        private bool IsActive { get; set; }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var i = GetComponent<Interactable>();
            if (string.IsNullOrWhiteSpace(levelName))
                levelName = "Level1";

            i.OnInteract += Toggle;
            i.SetText($"Go to {levelName}");
        }


        private void Toggle()
        {
            LevelLoader.Instance.LoadLevel(levelName);
        }


    }
}

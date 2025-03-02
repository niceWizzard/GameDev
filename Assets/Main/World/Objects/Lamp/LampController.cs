using Main.Lib;
using UnityEngine;

namespace Main.World.Objects.Lamp
{
    [RequireComponent(typeof(Interactable))]
    public class LampController : MonoBehaviour
    {
        [SerializeField] private Sprite activatedSprite;
        [SerializeField] private Sprite inactiveSprite;
        private SpriteRenderer _spriteRenderer;


        private bool IsActive { get; set; }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GetComponent<Interactable>().OnInteract += Toggle;
        }


        private void Toggle()
        {
            IsActive = !IsActive;
            _spriteRenderer.sprite = IsActive ? activatedSprite : inactiveSprite;
        }


    }
}

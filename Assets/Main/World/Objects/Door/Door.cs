using System;
using DG.Tweening;
using Main.World.Objects.Pedestal;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.World.Objects.Door
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftDoor;
        [SerializeField] private SpriteRenderer rightDoor;
        [SerializeField] private PedestalController pedestalController;
        private Collider2D _collider;

        private void Awake()
        {
            VerifyRequirements();
            _collider = GetComponent<Collider2D>();
            pedestalController.Activated += PedestalControllerOnActivated;
        }

        private void VerifyRequirements()
        {
            if(!leftDoor)
                Debug.LogError($"Left door is not set at {name}");
            if(!rightDoor)
                Debug.LogError($"Right door is not set at {name}");
            if(!pedestalController)
                throw new NullReferenceException($"Pedestal controller is not set at {name}");
        }

        private void OnDestroy()
        {
            pedestalController.Activated -= PedestalControllerOnActivated;
        }

        private void PedestalControllerOnActivated()
        {
            leftDoor.transform.DOMoveX(transform.position.x - 1, 0.8f);
            rightDoor.transform.DOMoveX(transform.position.x + 1, 0.8f);
            leftDoor.DOFade(0, 0.2f);
            rightDoor.DOFade(0, 0.2f);
            _collider.enabled = false;
        }
    }
}

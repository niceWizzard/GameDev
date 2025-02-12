using Main.Player;
using UnityEngine;

namespace Main.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField]  private PlayerController  player;
        [SerializeField] private AmmoHUDController ammoHUDController = null!;
        [SerializeField] private HealthHUDController healthHUDController = null!;

        private void Awake()
        {
            ammoHUDController.player = player;
            healthHUDController.Player = player;
        }
    }
}

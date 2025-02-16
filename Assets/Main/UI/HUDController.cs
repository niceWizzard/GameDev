using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main.UI
{
    public class HUDController : PrefabSingleton<HUDController>
    {
        [SerializeField] private PlayerController  player;
        [SerializeField] private AmmoHUDController ammoHUDController = null!;
        [SerializeField] private HealthHUDController healthHUDController = null!;
        public void SetPlayer(PlayerController player)
        {
            if(this.player)
                Debug.LogError("Player already exists");
            this.player = player;
            ammoHUDController.Setup(player);
            healthHUDController.Setup(player);
        }

    }
}

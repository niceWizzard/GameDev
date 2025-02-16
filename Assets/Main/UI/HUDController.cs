using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;

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
        }
        private void Start()
        {
            ammoHUDController.player = player;
            healthHUDController.Player = player;
        }

    }
}

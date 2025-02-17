using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main.UI
{
    public class HUDController : PrefabSingleton<HUDController>
    {
        private PlayerController  _player;
        [SerializeField] private AmmoHUDController ammoHUDController = null!;
        [SerializeField] private HealthHUDController healthHUDController = null!;
        
        public void SetPlayer(PlayerController player)
        {
            if(this._player || this._player != null)
                Debug.LogError("Player already exists");
            this._player = player;
            ammoHUDController.Setup(player);
            healthHUDController.Setup(player);
        }

    }
}

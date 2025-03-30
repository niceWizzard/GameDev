using Main.Lib.Level;
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

        protected override void Awake()
        {
            base.Awake();
            GameManager.LevelUnload += GameManagerOnLevelUnload;
        }

        private void GameManagerOnLevelUnload()
        {
            if(ammoHUDController)
                ammoHUDController.Disable();
            if (healthHUDController)
                healthHUDController.Disable();
        }

        public void EnterCutsceneMode()
        {
            ammoHUDController.Disable();
            healthHUDController.Disable();
        }

        public void ExitCutsceneMode()
        {
            ammoHUDController.Enable();
            healthHUDController.Enable();
        }


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

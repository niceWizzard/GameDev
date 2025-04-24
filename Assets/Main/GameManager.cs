using System;
using Main.Lib.Level;
using Main.Lib.Singleton;
using Main.UI;

namespace Main
{
    public class GameManager : Singleton<GameManager>
    {
        public static LevelManager CurrentLevel => Instance.CurrentLevelManager; 
        public static event Action<LevelManager> LevelLoaded; 
        public static event Action LevelUnload; 
        private LevelManager _levelManager;

        public static string DiedAtLevel { get; set; } = "";

        public LevelManager CurrentLevelManager => _levelManager;

        public void RegisterLevelManager(LevelManager levelManager)
        {
            _levelManager = levelManager;
            LevelLoaded?.Invoke(levelManager);
            BgMusic.PlayAmbientMusic();
        }
        
        public void UnregisterLevelManager()
        {
            _levelManager = null;
            LevelUnload?.Invoke();
            BgMusic.StopAll();
        }
        
        public static void LoadEssentials()
        {
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
            CutscenePanel.InitializePrefab("CutscenePanel");
            Dialog.InitializePrefab("Dialog");
            PrefabLoader.InitializePrefab("PrefabLoader");
            BgMusic.InitializePrefab("BgMusic");
        }

    }
}

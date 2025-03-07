using System;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using Main.Lib.Level;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;

namespace Main
{
    public class GameManager : Singleton<GameManager>
    {
        public static LevelManager CurrentLevel => Instance.CurrentLevelManager; 
        public static event Action<LevelManager> OnLevelLoaded; 
        public static event Action OnLevelUnload; 
        private LevelManager _levelManager;

        public LevelManager CurrentLevelManager => _levelManager;

        public void RegisterLevelManager(LevelManager levelManager)
        {
            _levelManager = levelManager;
            OnLevelLoaded?.Invoke(levelManager);
        }
        
        public void UnregisterLevelManager()
        {
            _levelManager = null;
            OnLevelUnload?.Invoke();
        }
        
        public static void LoadEssentials()
        {
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

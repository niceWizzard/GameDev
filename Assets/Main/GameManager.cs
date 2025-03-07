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

        public static bool HasBeenDefeated(UniqueId id)
        {
            return SaveManager.Instance.SaveGameData.DefeatedEnemies.Contains(id.Id);
        }

        public static void AddDefeated(UniqueId id)
        {
            _ = SaveManager.Instance.SaveDataAsync(saveGameData =>
                saveGameData with
                {
                    DefeatedEnemies = SaveManager.Instance.SaveGameData.DefeatedEnemies.Append(id.Id).ToList(),
                }    
            );
        }

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

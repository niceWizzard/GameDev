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
        }
        
        public void UnregisterLevelManager() => _levelManager = null;
        
        public static void LoadEssentials()
        {
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

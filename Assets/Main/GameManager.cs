using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;

namespace Main
{
    public class GameManager : Singleton<GameManager>
    {
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
        
        
        public static void LoadEssentials()
        {
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

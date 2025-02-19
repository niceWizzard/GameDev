using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine.EventSystems;

namespace Main
{
    public class GameManager : Singleton<GameManager>
    {
        public static bool HasBeenDefeated(UniqueId id)
        {
            return SaveManager.Instance.SaveGameData.defeatedEnemies.Contains(id.Id);
        }

        public static void AddDefeated(UniqueId id)
        {
            SaveManager.Instance.SaveGameData.defeatedEnemies.Add(id.Id);
            SaveManager.Instance.SaveData();
        }
        
        
        public static void LoadEssentials()
        {
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

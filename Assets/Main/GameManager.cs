using System.Collections.Generic;
using CleverCrow.Fluid.UniqueIds;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine.EventSystems;

namespace Main
{
    public class GameManager : Singleton<GameManager>
    {
        private static HashSet<string> _defeatedEnemies = new();

        public static bool HasBeenDefeated(UniqueId id)
        {
            return _defeatedEnemies.Contains(id.Id);
        }

        public static void AddDefeated(UniqueId id)
        {
            _defeatedEnemies.Add(id.Id);
        }
        
        public static void LoadEssentials()
        {
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

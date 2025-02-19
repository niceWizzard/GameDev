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
        private static HashSet<string> _defeatedEnemies = new();


        protected override void Awake()
        {
            base.Awake();
            _defeatedEnemies = SaveManager.Instance.SaveGameData.defeatedEnemies.ToHashSet();
        }

        public static bool HasBeenDefeated(UniqueId id)
        {
            return _defeatedEnemies.Contains(id.Id);
        }

        public static void AddDefeated(UniqueId id)
        {
            _defeatedEnemies.Add(id.Id);
            SaveManager.Instance.SaveGameData.defeatedEnemies = _defeatedEnemies.ToList();
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

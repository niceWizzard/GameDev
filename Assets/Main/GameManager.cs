using System;
using Main.Lib.Singleton;
using Main.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Main
{
    public class GameManager : Singleton<GameManager>
    {

        public static void LoadEssentials()
        {
            if (IsInitialized)
                return;
            Initialize();
            MainCamera.InitializePrefab("MainCamera");
            HUDController.InitializePrefab("HUD");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

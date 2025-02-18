using System;
using System.Collections;
using System.Collections.Generic;
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
            MainCamera.InitializePrefab("Camera Container");
            HUDController.InitializePrefab("HUD Canvas");
            LevelLoader.InitializePrefab("LevelLoader");
        }

    }
}

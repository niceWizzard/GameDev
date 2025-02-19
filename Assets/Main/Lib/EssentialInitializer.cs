using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib
{
    public class EssentialInitializer : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private static bool initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSceneLoad()
        {
            if (initialized) return;
            var instance = new GameObject("ScenePreloader").AddComponent<EssentialInitializer>();
            SaveManager.Initialize();
            SaveManager.Instance.LoadSaveGameData();
            GameManager.Initialize();
            GameManager.LoadEssentials();
        }

    }
}

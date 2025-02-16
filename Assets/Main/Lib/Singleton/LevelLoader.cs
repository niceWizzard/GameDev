using System;
using System.Collections;
using System.Security.Cryptography;
using Main.Player;
using Main.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Singleton
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        private HUDController _hudPrefab;
        public static event Action OnLevelChange;
        public static void Setup(
            HUDController hudPrefab) 
        {
            Instance._hudPrefab = hudPrefab;
        }

        public void LoadLevel(SceneAsset levelAsset)
        {
            StartCoroutine(LoadLevelCoroutine(levelAsset));
        }

        private static IEnumerator LoadLevelCoroutine(SceneAsset levelAsset)
        {
            yield return SceneManager.LoadSceneAsync(levelAsset.name);
            OnLevelChange?.Invoke();
        }
    }
}

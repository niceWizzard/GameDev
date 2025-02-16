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
        public static void Setup(
            HUDController hudPrefab) 
        {
            Instance._hudPrefab = hudPrefab;
        }

        public void LoadLevel(SceneAsset levelAsset)
        {
            StartCoroutine(LoadLevelCoroutine(levelAsset));
        }

        private IEnumerator LoadLevelCoroutine(SceneAsset levelAsset)
        {
            yield return SceneManager.LoadSceneAsync(levelAsset.name);
            var o = Instantiate(_hudPrefab);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            o.SetPlayer(player);
            MainCamera.Instance.Follow(player);
        }
    }
}

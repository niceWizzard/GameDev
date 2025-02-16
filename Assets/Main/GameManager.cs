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
        [SerializeField] private  HUDController hudPrefab;

        protected override void Awake()
        {
            base.Awake();
            LevelLoader.Setup(hudPrefab);
        }

        public static void Load()
        {
            if (IsInitialized)
                return;
            var a = Addressables.LoadSceneAsync("GameEssentials", LoadSceneMode.Additive).WaitForCompletion();
            SetupEssentials();
        }

        private static void SetupEssentials()
        {
            MainCamera.TryInitializePrefab();
        }
    }
}

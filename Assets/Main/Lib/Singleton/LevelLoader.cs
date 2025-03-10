#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    [Serializable]
    public class LevelMapping
    {
        public string levelName;
        public AssetReference scene;
    }
    public class LevelLoader : PrefabSingleton<LevelLoader>
    {
        [SerializeField] private Image blackScreen = null!;
        [SerializeField] private List<LevelMapping> levelMappings = null!;
        public static event Action<string>? OnLevelChange;
        

        protected override void Awake()
        {
            base.Awake();
            var color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color;
        }

        public string GetLevelGuid(string levelName) =>
            levelMappings.Find(v => v.levelName == levelName).scene.AssetGUID;

        public void LoadLevel(AssetReference levelRef)
        {
            StartCoroutine(LoadLevelCoroutine(levelRef));
        }
        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelCoroutine(levelName));
        }
        
        private static IEnumerator LoadLevelCoroutine(string levelName)
        {
            Instance.blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.25f);
            yield return Addressables.LoadSceneAsync(levelName).Yield();
            yield return new WaitForSecondsRealtime(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
            Time.timeScale = 1;
        }


        private static IEnumerator LoadLevelCoroutine(AssetReference levelRef)
        {
            Instance.blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.25f);
            yield return levelRef.LoadSceneAsync().Yield();
            yield return new WaitForSecondsRealtime(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic).SetUpdate(true);
            Time.timeScale = 1;
        }
    }
}

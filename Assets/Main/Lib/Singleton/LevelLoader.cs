#nullable enable
using System;
using System.Collections;
using System.Linq;
using CleverCrow.Fluid.UniqueIds;
using DG.Tweening;
using Main.Lib.Level;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class LevelLoader : PrefabSingleton<LevelLoader>
    {
        [SerializeField] private Image blackScreen = null!;
        public static event Action<string>? OnLevelChange;
        

        protected override void Awake()
        {
            base.Awake();
            var color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color;
        }

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
            Instance.blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(0.25f);
            yield return Addressables.LoadSceneAsync(levelName).Yield();
            yield return new WaitForSeconds(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic);
        }


        private static IEnumerator LoadLevelCoroutine(AssetReference levelRef)
        {
            Instance.blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(0.25f);
            yield return levelRef.LoadSceneAsync().Yield();
            yield return new WaitForSeconds(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic);
        }
    }
}

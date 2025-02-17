using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class LevelLoader : PrefabSingleton<LevelLoader>
    {
        [SerializeField] private Image blackScreen;
        public static event Action<string> OnLevelChange;

        protected override void Awake()
        {
            base.Awake();
            var color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color;
        }

        public void LoadLevel(string levelName)
        {
            blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            OnLevelChange?.Invoke(levelName);
            StartCoroutine(LoadLevelCoroutine(levelName));
        }

        private static IEnumerator LoadLevelCoroutine(string levelName)
        {
            yield return new WaitForSeconds(0.25f);
            Addressables.LoadSceneAsync(levelName).WaitForCompletion();
            yield return new WaitForSeconds(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic);
        }
    }
}

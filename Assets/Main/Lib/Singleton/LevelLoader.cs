using System;
using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
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

        public void LoadLevel(SceneAsset levelAsset)
        {
            blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            OnLevelChange?.Invoke(levelAsset.name);
            StartCoroutine(LoadLevelCoroutine(levelAsset));
        }

        private static IEnumerator LoadLevelCoroutine(SceneAsset levelAsset)
        {
            yield return new WaitForSeconds(0.25f);
            var operation = SceneManager.LoadSceneAsync(levelAsset.name);
            if(operation == null)
                throw new NullReferenceException("Cannot load level");
            while (!operation.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelAsset.name));
            yield return new WaitForSeconds(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic);
        }
    }
}

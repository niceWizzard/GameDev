#nullable enable
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main.World.Objects.CompletionStatue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class LevelLoader : PrefabSingleton<LevelLoader>
    {
        [SerializeField] private Image blackScreen = null!;
        
        private List<string> _sceneNames = new();
        
        protected override void Awake()
        {
            base.Awake();
            var color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color;
            
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            for (var i = 0; i < sceneCount; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                _sceneNames.Add(sceneName);
            }
        }


        public void LoadLevel(string levelName)
        {
            if (!_sceneNames.Contains(levelName))
            {
                Debug.LogError($"Scene <{levelName}> not found. Have you added it to the scene list?");
                return;
            };
            StartCoroutine(LoadLevelCoroutine(levelName));
        }
        
        private IEnumerator LoadLevelCoroutine(string levelName)
        {
            Time.timeScale = 0;
            Instance.blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(gameObject);
            yield return new WaitForSecondsRealtime(0.25f);
            yield return SceneManager.LoadSceneAsync(levelName).Yield();
            yield return new WaitForSecondsRealtime(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(gameObject);
            Time.timeScale = 1;
        }

    }
}

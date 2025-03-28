#nullable enable
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Main.Lib.Save;
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

        public void LoadHub()
        {
            if (SaveManager.Instance.SaveGameData.CompletedTutorial)
            {
                LoadLevel("HubLevel");
            }
            else
            {
                if (SaveManager.Instance.SaveGameData.PlayedCutScenes.Contains("TutorialHub Cutscene"))
                {
                    LoadLevel("TutorialHub");
                }
                else
                {
                    LoadLevel("TutorialHub Cutscene");
                }
            }
        }

        public void LoadLevel(string levelName)
        {
            if (!_sceneNames.Contains(levelName))
            {
                Debug.LogError($"Scene <{levelName}> not found. Have you added it to the scene list?");
                return;
            };
            _ = _LoadLevel(levelName);
        }
        
        private async UniTask _LoadLevel(string levelName)
        {
            Time.timeScale = 0;
            await CutscenePanel.FadeInAsync();
            await SceneManager.LoadSceneAsync(levelName);
            Time.timeScale = 1;
            await CutscenePanel.FadeOutAsync();
        }

    }
}

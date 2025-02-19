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
    [Serializable]
    public struct LevelDoorMapping
    {
        public string levelName;
        public string[] doors;
    }
    public class LevelLoader : PrefabSingleton<LevelLoader>
    {
        [SerializeField] private Image blackScreen = null!;
        [SerializeField] private LevelDoorMapping[] levelDoorMap = null!;
        public static event Action<string>? OnLevelChange;
        
        private string? DoorToLoadFrom { get; set; }

        public string? GetDoorToLoad()
        {
            var a = DoorToLoadFrom;
            DoorToLoadFrom = null;
            return a;
        }

        protected override void Awake()
        {
            base.Awake();
            var color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color;
            
        }

        public void GoToLevel(string unique)
        {
            blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            OnLevelChange?.Invoke(unique);
            DoorToLoadFrom = unique;
            StartCoroutine(LoadLevelCoroutine(GetSceneNameFromDoor(unique)));
        }

        private string GetSceneNameFromDoor(string unique)
        {
            var sceneMapping = levelDoorMap
                .FirstOrDefault(x => x.doors.Any(d => d == unique));
            if (sceneMapping.levelName != null) return sceneMapping.levelName;
            Debug.LogError($"Scene not found from door id: <{unique}>");
            return string.Empty;
        }
        
        private static IEnumerator LoadLevelCoroutine(string levelName)
        {
            yield return new WaitForSeconds(0.25f);
            yield return Addressables.LoadSceneAsync(levelName).Yield();
            yield return new WaitForSeconds(0.01f);
            Instance.blackScreen.DOFade(0, 0.25f).SetEase(Ease.InCubic);
        }
    }
}

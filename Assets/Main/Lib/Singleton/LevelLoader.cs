using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Main.Lib.Level;
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
        public DoorIdentifier[] doors;
    }
    public class LevelLoader : PrefabSingleton<LevelLoader>
    {
        [SerializeField] private Image blackScreen;
        [FormerlySerializedAs("_levelDoorMap")] [SerializeField] private LevelDoorMapping[] levelDoorMap;
        public static event Action<DoorIdentifier> OnLevelChange;

        protected override void Awake()
        {
            base.Awake();
            var color = blackScreen.color;
            color.a = 0;
            blackScreen.color = color;
        }

        public void GoToLevel(DoorIdentifier door)
        {
            blackScreen.DOFade(1, 0.25f).SetEase(Ease.InCubic);
            OnLevelChange?.Invoke(door);
            StartCoroutine(LoadLevelCoroutine(GetSceneNameFromDoor(door)));
        }

        private string GetSceneNameFromDoor(DoorIdentifier door)
        {
            var sceneName = levelDoorMap.ToList().Find(x => x.doors.Contains(door)).levelName;
            if(sceneName == null) 
                Debug.LogError($"Scene not found: {door.debugIdentifier}");
            return sceneName;
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

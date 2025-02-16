using System;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Main.Lib.Singleton
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        public static event Action OnLevelChange;

        public void LoadLevel(SceneAsset levelAsset)
        {
            StartCoroutine(LoadLevelCoroutine(levelAsset));
        }

        private static IEnumerator LoadLevelCoroutine(SceneAsset levelAsset)
        {
            yield return SceneManager.LoadSceneAsync(levelAsset.name);
            OnLevelChange?.Invoke();
        }
    }
}

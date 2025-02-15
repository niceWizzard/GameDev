using System;
using Main.Lib.Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private SceneAsset firstScene;
        private void Awake()
        {
            LevelLoader.FirstLoad();
            SceneManager.LoadScene(firstScene.name);
        }
    }
}

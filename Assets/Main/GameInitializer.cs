using System;
using Main.Lib.Singleton;
using Main.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private SceneAsset firstScene;
        [SerializeField] private  HUDController hudPrefab;
        [SerializeField] private MainCamera mainCameraPrefab;

        private void Awake()
        {
            LevelLoader.Setup(hudPrefab);
            LevelLoader.Instance.LoadLevel(firstScene);
            Instantiate(mainCameraPrefab);
        }
    }
}

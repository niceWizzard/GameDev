using System;
using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;

namespace Main.Lib.Level
{
    public class LevelManager : MonoBehaviour
    {
        private void Awake()
        {
            MainCamera.TryInitializePrefab();
            MainCamera.Instance.Follow(FindAnyObjectByType<PlayerController>());
        }
    }
}

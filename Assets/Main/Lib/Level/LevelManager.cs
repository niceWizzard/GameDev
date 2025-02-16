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
            GameManager.Load();
            MainCamera.Instance.Follow(FindAnyObjectByType<PlayerController>());
            var player = FindAnyObjectByType<PlayerController>();
            player.transform.position = transform.position;
            MainCamera.Instance.MoveTo(transform.position);
        }


    }
}

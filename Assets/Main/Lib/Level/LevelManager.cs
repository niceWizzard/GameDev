using System;
using Main.Lib.Singleton;
using Main.Player;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Level
{
    public class LevelManager : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.LoadEssentials();
            var player = FindAnyObjectByType<PlayerController>();
            player.transform.position = transform.position;
            MainCamera.Instance.MoveTo(transform.position);
            MainCamera.Instance.Follow(player);
            HUDController.Instance.SetPlayer(player);
        }

    }
}

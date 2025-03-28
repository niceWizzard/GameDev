using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Level
{
    public class TutorialHub : MonoBehaviour
    {
        [SerializeField] private Transform safeSpawn;
        private void Awake()
        {
            var player = FindAnyObjectByType<PlayerController>();
            player.Position = safeSpawn.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
            {
                DialogSystem.ShowDialogWithButtons("Pausing?", new List<(string, Func<UniTask>)>()
                {
                    ("Continue", DialogSystem.CloseDialogAsync),
                    ("Menu",  () =>
                    {
                        DialogSystem.CloseDialog();
                        SceneManager.LoadScene("Startup");
                        return UniTask.CompletedTask;
                    }),
                    ("Quit", () =>
                    {
                        Application.Quit();
                        return UniTask.CompletedTask;
                    }),
                });
            }
        }
    }
}

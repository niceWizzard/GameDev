using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.Player;
using Main.World.Objects.Lamp;
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

        private void Start()
        {
            var activatedAll = GetComponents<LevelLampController>().All(v => v.IsActive);
            if (!activatedAll)
                return;
            SaveManager.Instance.SaveData(v => v with { CompletedTutorial = true });
            LevelLoader.Instance.LoadHub();
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

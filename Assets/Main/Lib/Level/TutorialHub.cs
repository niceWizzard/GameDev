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
            var lamps = FindObjectsByType<SealLampController>(FindObjectsSortMode.InstanceID);
            if (!string.IsNullOrEmpty(GameManager.DiedAtLevel))
            {
                foreach (var lamp in lamps)
                {
                    if (lamp.LevelName != GameManager.DiedAtLevel) continue;
                    var player = FindAnyObjectByType<PlayerController>();
                    player.Position = lamp.transform.position + Vector3.right * 1.5f;
                    GameManager.DiedAtLevel = "";
                    break;
                }
            }
            var activatedAll = lamps.All(v => v.IsActive);
            if (!activatedAll)
                return;
            _ = DoHasActivatedAllLamps();
        }

        private async UniTask DoHasActivatedAllLamps()
        {
            await UniTask.WaitForSeconds(0.1f, cancellationToken: destroyCancellationToken);
            DialogSystem.ShowMultiDialog(new List<string>()
            {
                "You are now ready, prince.",
                "Good luck! HEHEHE...."
            }, "Old man");
            await DialogSystem.AsyncAwaitMultiDialogClose();
            await SaveManager.Instance.SaveDataAsync(v => v with { CompletedTutorial = true });
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

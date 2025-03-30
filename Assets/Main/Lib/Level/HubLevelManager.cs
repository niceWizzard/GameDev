using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Main.Lib.Singleton;
using Main.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubLevelManager : MonoBehaviour
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
                    LevelLoader.Instance.LoadMenu();
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

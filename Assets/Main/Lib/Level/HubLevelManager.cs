using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Main;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.Player;
using Main.World.Objects.Lamp;
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

    private void Start()
    {
        if (string.IsNullOrEmpty(GameManager.DiedAtLevel)) return;
        
        var lamps = FindObjectsByType<SealLampController>(FindObjectsSortMode.InstanceID);
        foreach (var lamp in lamps)
        {
            if (lamp.LevelName != GameManager.DiedAtLevel) continue;
            var player = FindAnyObjectByType<PlayerController>();
            player.Position = lamp.transform.position + Vector3.right * 1.5f;
            GameManager.DiedAtLevel = "";
            break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            Dialog.CreateDialog("Pausing?", new List<(string, Action)>()
            {
                ("Continue", null),
                ("Menu",  () =>
                {
                    SceneManager.LoadScene("Startup");
                }),
                ("Quit", Application.Quit),
            });
        }
    }
}

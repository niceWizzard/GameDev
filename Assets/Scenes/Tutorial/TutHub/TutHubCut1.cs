using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutHubCut1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        HUDController.Instance.EnterCutsceneMode();
    }

    public void StartDialog()
    {
        DialogSystem.ShowMultiDialog(new List<string>()
        {
            "Wake up prince",
            "You need to escape this dungeon.",
            "Follow me.",
        }, "Old man");
    }

    public void StartDialog2()
    {
        DialogSystem.ShowMultiDialog(new List<string>()
        {
            "These are the Seal Lamps",
            "Listen to me, prince, these lamps are scattered all over the dungeon.",
            "You must light every lamps to break the seal binding you here.",
            "Take this gun to protect yourself against monsters"
        }, "Old man");
    }
    
    public void StartDialog3()
    {
        DialogSystem.ShowMultiDialog(new List<string>()
        {
            "Every lamps propose a challenge for you to defeat.",
            "You will enter an artificial world",
            "YOU MUST BREAK THE STATUE IN THAT WORLD!",
            "Try it now, prince.",
        }, "Old man");

    }
    
    public void GoToTutorial1()
    {
        SaveManager.Instance.SaveData(v => v with
        {
            PlayedCutScenes = v.PlayedCutScenes.Append(SceneManager.GetActiveScene().name).ToHashSet()
        });
        LevelLoader.Instance.LoadLevel("Tut1");
        Invoke(nameof(ExitCutscene), 0.2f);
    }

    private void ExitCutscene()
    {
        HUDController.Instance.ExitCutsceneMode();
    }
    
    
}

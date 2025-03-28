using System;
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
        _ = _StartDialog();
    }
    private async UniTask _StartDialog()
    {
        await DialogSystem.ShowDialogAsync("Wake up prince", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("You need to escape this dungeon.", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("Follow me.", "Old man");
    }

    public void StartDialog2()
    {
        _ = _StartDialog2();
    }
    
    private async UniTask _StartDialog2()
    {
        await DialogSystem.ShowDialogAsync("These are the Seal Lamps", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("Listen to me, prince, these lamps are scattered all over the dungeon.", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("You must light every lamps to break the seal binding you here.", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("Take this gun to protect yourself against monsters", "Old man");
        await DialogSystem.WaitForClose();
    }
    
    public void StartDialog3()
    {
        _ = _StartDialog3();
    }
    
    private async UniTask _StartDialog3()
    {
        await DialogSystem.ShowDialogAsync("Every lamps propose a challenge for you to defeat.", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("You will enter an artificial world", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("YOU MUST BREAK THE STATUE IN THAT WORLD!", "Old man");
        await DialogSystem.WaitForClose();
        await DialogSystem.ShowDialogAsync("Try it now, prince.", "Old man");
        await DialogSystem.WaitForClose();
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

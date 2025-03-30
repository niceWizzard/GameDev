using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Main.Lib.Cutscene;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Tutorial
{
    public class TutorialHubCutscene1 : CutscenePlayer
    {
        [SerializeField] private List<Transform> playerPositions;
        [SerializeField] private List<Transform> wizardPositions;
        [SerializeField] private List<Transform> playerWalk;
        [SerializeField] private List<Transform> wizardWalk;
        [SerializeField] private CutsceneEntity playerEntity;
        [SerializeField] private CutsceneEntity wizardEntity;

        [SerializeField] private GameObject playerGun;


        private void Start()
        {
            playerEntity.Position = playerPositions[0].position;
            wizardEntity.Position = wizardPositions[0].position;
            playerGun.SetActive(false);
            _ = StartDialog();
        }

        private async UniTask StartDialog()
        {
            await Wait(1);
            DialogSystem.ShowDialog("Wake up prince.");
            await Wait(1.5f);
            await DialogSystem.AsyncAwaitForClose();
            await CutscenePanel.HideAsync();
            await DialogSystem.AsyncAwaitForClose();
            DialogSystem.ShowMultiDialog(new List<string>()
            {
                "You need to escape this dungeon.",
                "Follow me.",
            }, "Old man");
            await DialogSystem.AsyncAwaitMultiDialogClose();
            wizardEntity.SpriteRenderer.flipX = true;
            await Wait(0.5f);
            wizardEntity.WalkTo(wizardWalk[0].position, 1);
            await Wait(0.5f);
            playerEntity.WalkTo(playerWalk[0].position, 1);
            playerEntity.Play("MoveGun");
            await Wait(1);
            await CutscenePanel.ShowAsync();

            playerEntity.Play("Idle");
            wizardEntity.CancelWalk();
            playerEntity.CancelWalk();
            playerEntity.Position = playerPositions[1].position;
            wizardEntity.Position = wizardPositions[1].position;
            SetCamera(1);
            
            await Wait(2);
            await CutscenePanel.HideAsync();
            DialogSystem.ShowMultiDialog(new List<string>()
            {
                "These are the Seal Lamps",
                "Listen to me, prince, these lamps are scattered all over the dungeon.",
                "You must light every lamps to break the seal binding you here.",
                "Take this gun to protect yourself against monsters"
            }, "Old man");
            await DialogSystem.AsyncAwaitMultiDialogClose();
            
            await Wait(2.5f);
            playerGun.SetActive(true);
            playerEntity.Play("IdleGun");

            DialogSystem.ShowMultiDialog(new List<string>()
            {
                "Every lamps propose a challenge for you to defeat.",
                "You will enter an artificial world",
                "YOU MUST BREAK THE STATUE IN THAT WORLD!",
                "Try it now, prince.",
            }, "Old man");
            await DialogSystem.AsyncAwaitMultiDialogClose();
            
            
            playerEntity.WalkTo(playerWalk[1].position, 1.5f);
            playerEntity.Play("MoveGun");
            await playerEntity.AsyncWaitToReachPosition();
            playerEntity.Play("IdleGun");
            
            await Wait(0.5f);
            await SaveManager.Instance.SaveDataAsync(v => v with
            {
                PlayedCutScenes = v.PlayedCutScenes.Append(SceneManager.GetActiveScene().name).ToHashSet()
            });
            LevelLoader.Instance.LoadLevel("Tut1");
            await Wait(0.2f);
            HUDController.Instance.ExitCutsceneMode();
        }

        
    }
}

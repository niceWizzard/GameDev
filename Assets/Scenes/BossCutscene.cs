using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Main.Lib.Cutscene;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class BossCutscene : CutscenePlayer
    {
        [SerializeField] private List<Transform> playerPositions;
        [SerializeField] private List<Transform> bossPositions;
        
        [SerializeField] private CutsceneEntity playerEntity;
        [SerializeField] private CutsceneEntity bossEntity;
        
        private void Start()
        {
            playerEntity.Position = playerPositions[0].position;
            bossEntity.Position = bossPositions[0].position;
            playerEntity.SpriteRenderer.flipX = true;
            
            _ = StartDialog();
        }

        private async UniTask StartDialog()
        {
            await Wait(1f);
            await Dialog.CreateDialog("....... ... *growl* ...", "..", false);
            await CutscenePanel.HideAsync();
            
            await Wait(0.5f);
            playerEntity.WalkTo(playerPositions[1].position, 4);
            bossEntity.WalkTo(bossPositions[1].position, 2);
            playerEntity.Play("MoveGun");
            bossEntity.Play("Move");
            MainCamera.Instance.Follow(playerEntity, false);
            await playerEntity.AsyncWaitToReachPosition();
            MainCamera.Instance.Unfollow();
            playerEntity.Play("IdleGun");
            bossEntity.Play("Idle");
            await Wait(1.5f);
            
            await Dialog.CreateDialog("....... invader ...", "..", false);
            
            await SaveManager.Instance.SaveDataAsync(v => v with
            {
                PlayedCutScenes = v.PlayedCutScenes.Append(SceneManager.GetActiveScene().name).ToHashSet()
            });
            LevelLoader.Instance.LoadLevel("Boss");
            await Wait(0.2f);
            HUDController.Instance.ExitCutsceneMode();
        }
        


    }
}

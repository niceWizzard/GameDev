using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Lib.Cutscene;
using Main.Lib.Save;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Ending
{
    public class EndingCutscene : CutscenePlayer
    {
        [SerializeField] private List<Transform> playerPositions;
        
        [SerializeField] private CutsceneEntity playerEntity;
        [SerializeField] private CutsceneEntity wizardEntity;
        
        private void Start()
        {
            playerEntity.Position = playerPositions[0].position;
            var color = wizardEntity.SpriteRenderer.color;
            color.a = 0;
            wizardEntity.SpriteRenderer.color = color;
            _ = StartDialog();
        }

        private async UniTask StartDialog()
        {
            await Wait(0.5f);
            await CutscenePanel.HideAsync();
            playerEntity.Play("MoveGun");
            playerEntity.WalkTo(playerPositions[1].position, 2.5f);
            
            await UniTask.WhenAll(
                wizardEntity.SpriteRenderer.DOFade(1, 0.5f).SetDelay(2.5f).SetLink(gameObject).AsyncWaitForCompletion().AsUniTask(),
                playerEntity.AsyncWaitToReachPosition()
            );
            
            playerEntity.Play("IdleGun");
            await Wait(0.5f);
            await Dialog.CreateDialogs(new List<string>()
            {
                "HAHAHAHA",
                "You have finally released my seals young prince...",
                "Thank you for your service!",
                "This world shall know pain!"
            }, "The Cursed Wizard", pause: false);
            await Wait(0.1f);
            await wizardEntity.SpriteRenderer.DOFade(0, 0.5f).SetLink(gameObject).AsyncWaitForCompletion();
            await Wait(1.5f);
            LevelLoader.Instance.LoadLevel("Credits");
            await Wait(0.2f);
            HUDController.Instance.ExitCutsceneMode();

        }
    }
}

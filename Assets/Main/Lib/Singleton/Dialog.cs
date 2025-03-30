using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class Dialog : PrefabSingleton<Dialog>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup dialogPanel;
        [SerializeField] private GameObject dialogButtonsContainer;
        [SerializeField] private Button dialogButtonTemplate;
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private TMP_Text senderText;
        [SerializeField] private TMP_Text continueText;
        
        private UniTaskCompletionSource _dialogClosedCompletionSource;
        private bool _isAnimatingText = false;
        private string _currentDialogMessage;

        protected override void Awake()
        {
            base.Awake();
            ResetDialog();
        }

        private static async UniTask CreateDialog(string message, string sender)
        {
            await Instance.AnimateDialogPanelOpening();
            _ = Instance.AnimateDialogText(message, sender);
            await UniTask.WaitUntil(() =>
            {
                if (!Input.GetKeyDown(KeyCode.Space))
                    return false;
                if (Instance._isAnimatingText)
                {
                    Instance._isAnimatingText = false;
                    Instance.dialogText.text = Instance._currentDialogMessage;
                    return false;
                }
                return true;
            });
            await Instance.AnimateDialogClosing();
            Instance._dialogClosedCompletionSource.TrySetResult();
        }

        public static async UniTask CreateDialogs(List<string> messages, string sender)
        {
            foreach (var message in messages)
            {
                await CreateDialog(message, sender);
            }
        }


        private void ResetDialog()
        {
            canvas.gameObject.SetActive(true);
            continueText.gameObject.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            senderText.text = "";
            dialogText.text = "";
            dialogPanel.transform.localScale *= 0;
        }

        private static void AssignCompletionSource()
        {
            Instance._dialogClosedCompletionSource = new UniTaskCompletionSource();
            Instance.destroyCancellationToken.Register(() =>
            {
                Instance._dialogClosedCompletionSource.TrySetCanceled();
            });
        }

        private async UniTask AnimateDialogText(string message, string sender)
        {
            _currentDialogMessage = message;
            _isAnimatingText = true;
            var t = "";
            foreach (var c in message.TakeWhile(c => _isAnimatingText))
            {
                t += c;
                dialogText.text = t;
                await UniTask.WaitForSeconds(0.05f, ignoreTimeScale:true, cancellationToken:destroyCancellationToken);
            }
            continueText.gameObject.SetActive(true);
        }
        
        private async UniTask AnimateDialogClosing()
        {
            await dialogPanel.DOFade(0, .1f)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .SetLink(Instance.gameObject)
                .AsyncWaitForCompletion();
            ResetDialog();
        }
        
        private UniTask AnimateDialogPanelOpening()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(
                dialogPanel.transform.DOScale(Vector3.one, 0.1f)
                    .SetEase(Ease.InCubic)
                    .SetUpdate(true)
                    .SetLink(Instance.gameObject)
            );
            dialogPanel.alpha = 0;
            sequence.Append(
                dialogPanel.DOFade(1, 0.2f)
                    .SetEase(Ease.InCubic)
                    .SetUpdate(true)
                    .SetLink(Instance.gameObject)
            );
            return sequence.Play().SetUpdate(true).AsyncWaitForCompletion().AsUniTask();
        }
        
        
        
    }
}

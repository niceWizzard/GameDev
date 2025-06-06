using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        [SerializeField] private AudioSource dialogAudioSource;
        
        private bool _isAnimatingText = false;
        private string _currentDialogMessage;
        private bool _isDialogOpen = false;
        private List<Button> _dialogButtons = new();

        protected override void Awake()
        {
            base.Awake();
            ResetDialog();
        }

        public static async UniTask CreateDialog(string message, string sender="", bool pause=true)
        {
            if(pause)
                Time.timeScale = 0;
            Instance._isDialogOpen = true;
            await Instance.AnimateDialogPanelOpening();
            _ = Instance.AnimateDialogText(message, sender);
            await UniTask.WaitUntil(() =>
            {
                if (!Input.GetKeyDown(KeyCode.Space))
                    return false;
                if (!Instance._isAnimatingText) 
                    return true;
                Instance._isAnimatingText = false;
                Instance.dialogText.text = Instance._currentDialogMessage;
                return false;
            }, cancellationToken: Instance.destroyCancellationToken);
            await CloseDialog();
        }

        public static async UniTask CreateDialogs(List<string> messages, string sender, bool pause=true)
        {
            foreach (var message in messages)
            {
                await CreateDialog(message, sender, pause);
            }
        }

        public static void CreateDialog(string message, List<(string, Action)> buttons, string sender = "", bool pause=true)
        {
            if(pause)
                Time.timeScale = 0;
            ClearButtons();
            foreach (var (text, action) in buttons)
            {
                var button = Instantiate(Instance.dialogButtonTemplate, Instance.dialogPanel.transform);
                button.transform.SetParent(Instance.dialogButtonsContainer.transform);
                button.GetComponentInChildren<TMP_Text>().text = text;
                button.onClick.AddListener(() =>
                {
                    _ = CloseDialog();
                    action?.Invoke();
                });
                button.gameObject.SetActive(true);
                button.transform.localScale = Vector3.one;
                Instance._dialogButtons.Add(button);
            }

            // Set navigation
            var dialogButtons = Instance._dialogButtons;
            EventSystem.current.SetSelectedGameObject(dialogButtons[0].gameObject);
            for (var i = 0; i < dialogButtons.Count; i++)
            {
                var button = dialogButtons[i];
                var navigation = button.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                // Set up "Up" and "Down" navigation
                if (i > 0)
                    navigation.selectOnUp = dialogButtons[i - 1];
                if (i < buttons.Count - 1)
                    navigation.selectOnDown = dialogButtons[i + 1];
                if (dialogButtons.Count > 1)
                {
                    if(i == 0)
                        navigation.selectOnUp = dialogButtons[^1];
                    if (i == buttons.Count - 1)
                        navigation.selectOnDown = dialogButtons[0];
                }
                button.navigation = navigation;
            }
            
            Instance.dialogButtonsContainer.SetActive(true);
            _ = AsyncAnimateDialog();
            return;

            async UniTask AsyncAnimateDialog()
            {
                Instance._isDialogOpen = true;
                await Instance.AnimateDialogPanelOpening();
                _ = Instance.AnimateDialogText(message, sender);
            }
        }

        private static void ClearButtons()
        {
            Instance._dialogButtons.ForEach(b => Destroy(b.gameObject));
            Instance._dialogButtons.Clear();
        }

        public static async UniTask CloseDialog()
        {
            Instance._isDialogOpen = true;
            ClearButtons();
            await Instance.AnimateDialogClosing();
        }

        private void ResetDialog()
        {
            canvas.gameObject.SetActive(false);
            continueText.gameObject.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            senderText.text = "";
            dialogText.text = "";
            dialogPanel.transform.localScale *= 0;
            Time.timeScale = 1;
        }


        private async UniTask AnimateDialogText(string message, string sender)
        {
            dialogAudioSource.Play();
            _currentDialogMessage = message;
            _isAnimatingText = true;
            var t = "";
            foreach (var c in message.TakeWhile(c => _isAnimatingText))
            {
                t += c;
                dialogText.text = t;
                await UniTask.WaitForSeconds(0.05f, ignoreTimeScale:true, cancellationToken:destroyCancellationToken);
            }
            dialogAudioSource.Stop();
            _isAnimatingText = false;
            if(_dialogButtons.Count == 0)
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
            canvas.gameObject.SetActive(true);
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

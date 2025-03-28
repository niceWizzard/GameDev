using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class DialogSystem : PrefabSingleton<DialogSystem>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image dialogPanel;
        [SerializeField] private GameObject dialogButtonsContainer;
        [SerializeField] private Button dialogButtonTemplate;
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private TMP_Text senderText;
        [SerializeField] private TMP_Text continueText;

        private List<Button> dialogButtons = new List<Button>();
        
        private enum DialogState
        {
            Closed,
            Closing,
            Opening,
            DialogAnimation,
            Completed,
        }
        
        private DialogState _dialogState = DialogState.Closed;
        
        private string _currentDialog = "";
        private int _currentButtonIndex;

        protected override void Awake()
        {
            base.Awake();
            canvas.gameObject.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            dialogText.text = "";
            senderText.text = "";
            continueText.gameObject.SetActive(false);
        }

        public static void ShowDialog(string message, string sender="")
        {
            if (Instance._dialogState != DialogState.Closed)
            {
                Debug.LogWarning("DialogSystem is already showing");
                return;
            }
            _ = Instance._ShowDialog(message, sender);
        }

        public static async UniTask ShowDialogAsync(string message, string sender = "")
        {
            if (Instance._dialogState != DialogState.Closed)
            {
                Debug.LogWarning("DialogSystem is already showing");
                return;
            }
            ShowDialog(message, sender);
            await UniTask.WaitUntil(() => Instance._dialogState == DialogState.Completed, cancellationToken: Instance.destroyCancellationToken);
        }
        
        public static void ShowDialogWithButtons(string message, List<(string, Func<UniTask>)> buttons,string sender="")
        {
            if (Instance._dialogState != DialogState.Closed)
            {
                Debug.LogWarning("DialogSystem is already showing");
                return;
            }
            _ = Instance._ShowDialogWithButtons(message, buttons, sender);
        }

        public static async UniTask ShowDialogWithButtonsAsync(string message, List<(string, Func<UniTask>)> buttons,
            string sender = "")
        {
            if (Instance._dialogState != DialogState.Closed)
            {
                Debug.LogWarning("DialogSystem is already showing");
                return;
            }
            ShowDialogWithButtons(message, buttons, sender);
            await UniTask.WaitUntil(() => Instance._dialogState == DialogState.Completed, cancellationToken: Instance.destroyCancellationToken);
        }


        private async UniTask _ShowDialogWithButtons(string message, List<(string, Func<UniTask>)> buttons, string sender = "")
        {
            Time.timeScale = 0;
            Reset();
            _dialogState = DialogState.Opening;
            await AnimateDialogPanelOpen();
            
            foreach (var (text, action) in buttons)
            {
                var button = Instantiate(dialogButtonTemplate, dialogPanel.transform);
                button.transform.SetParent(dialogButtonsContainer.transform);
                button.GetComponentInChildren<TMP_Text>().text = text;
                button.onClick.AddListener(() => action());
                button.gameObject.SetActive(true);
                dialogButtons.Add(button);
            }
            EventSystem.current.SetSelectedGameObject(dialogButtons[0].gameObject);
            for (int i = 0; i < dialogButtons.Count; i++)
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
            await AnimateDialog(message, sender);
            dialogButtonsContainer.SetActive(true);
        }

        private async UniTask AnimateDialogPanelOpen()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(
                dialogPanel.transform.DOScale(Vector3.one, 0.1f)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .SetLink(Instance.gameObject)
            );
            var color = dialogPanel.color;
            color.a = 0;
            dialogPanel.color = color;
            sequence.Join(
                dialogPanel.DOFade(1, 0.2f)
                    .SetEase(Ease.InCubic)
                    .SetUpdate(true)
                    .SetLink(Instance.gameObject)
            );
            await sequence.Play().SetUpdate(true).AsyncWaitForCompletion();
        }

        private void Reset()
        {
            canvas.gameObject.SetActive(true);
            continueText.gameObject.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            dialogPanel.transform.localScale *= 0;
            dialogButtons.ForEach(b => Destroy(b.gameObject));
            dialogButtons.Clear();
        }


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            switch (_dialogState)
            {
                case DialogState.DialogAnimation:
                    _dialogState = DialogState.Completed;
                    dialogText.text = _currentDialog;
                    break;
                case DialogState.Completed when dialogButtons.Count == 0:
                    _ = _CloseDialog();
                    break;
            }
        }
        

        public static void CloseDialog()
        {
            if (Instance._dialogState != DialogState.Completed) return;
            _ = Instance._CloseDialog();
        }

        public static async UniTask WaitForClose()
        {
            await UniTask.WaitUntil(() => Instance._dialogState == DialogState.Closed, cancellationToken: Instance.destroyCancellationToken);
        }

        public static async UniTask CloseDialogAsync()
        {
            if (Instance._dialogState != DialogState.Completed) return;
            CloseDialog();
            await UniTask.WaitUntil(() => Instance._dialogState == DialogState.Closed, cancellationToken: Instance.destroyCancellationToken);
        }

        private async UniTask _CloseDialog()
        {
            _dialogState = DialogState.Closing;
            dialogPanel.transform.localScale *= 0;
            _dialogState = DialogState.Closed;
            canvas.gameObject.SetActive(false);
            senderText.text = "";
            dialogText.text = "";
            Time.timeScale = 1;
        }

        

        private async UniTask _ShowDialog(string message, string sender = "")
        {
            Reset();
            Time.timeScale = 0;
            _dialogState = DialogState.Opening;
            await AnimateDialogPanelOpen();
            await AnimateDialog(message, sender);
        }

        private async UniTask AnimateDialog(string message, string sender)
        {
            _dialogState = DialogState.DialogAnimation;
            senderText.text = sender;
            _currentDialog = message;
            var t = "";
            foreach (var c in message)
            {
                if (_dialogState != DialogState.DialogAnimation)
                    break;
                t += c;
                dialogText.text = t;
                await UniTask.WaitForSeconds(0.05f, ignoreTimeScale:true, cancellationToken:destroyCancellationToken);
            }
            _dialogState = DialogState.Completed;
            if(dialogButtons.Count == 0)
                continueText.gameObject.SetActive(true);
        }
    }
}

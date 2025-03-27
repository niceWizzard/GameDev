using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class DialogSystem : PrefabSingleton<DialogSystem>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject dialogPanel;
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
        
        public static void ShowDialogWithButtons(string message, List<(string, Action)> buttons,string sender="")
        {
            if (Instance._dialogState != DialogState.Closed)
            {
                Debug.LogWarning("DialogSystem is already showing");
                return;
            }
            _ = Instance._ShowDialogWithButtons(message, buttons, sender);
            
        }

        private async UniTask _ShowDialogWithButtons(string message, List<(string, Action)> buttons, string sender = "")
        {
            Time.timeScale = 0;
            canvas.gameObject.SetActive(true);
            continueText.gameObject.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            dialogPanel.transform.localScale *= 0;
            dialogButtons.ForEach(b => Destroy(b.gameObject));
            dialogButtons.Clear();
            _dialogState = DialogState.Opening;
            await dialogPanel.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
            await AnimateDialog(message, sender);
            foreach (var (text, action) in buttons)
            {
                var button = Instantiate(dialogButtonTemplate, dialogPanel.transform);
                button.transform.parent = dialogButtonsContainer.transform;
                button.GetComponentInChildren<TMP_Text>().text = text;
                button.onClick.AddListener(() => action());
                button.gameObject.SetActive(true);
                dialogButtons.Add(button);
            }
            dialogButtonsContainer.SetActive(true);
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
                case DialogState.Completed:
                    _ = _CloseDialog();
                    break;
            }
        }

        public static void CloseDialog()
        {
            if (Instance._dialogState != DialogState.Completed) return;
            _ = Instance._CloseDialog();
        }

        private async UniTask _CloseDialog()
        {
            _dialogState = DialogState.Closing;
            await dialogPanel.transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
            _dialogState = DialogState.Closed;
            canvas.gameObject.SetActive(false);
            senderText.text = "";
            dialogText.text = "";
            Time.timeScale = 1;
        }

        private async UniTask _ShowDialog(string message, string sender = "")
        {
            Time.timeScale = 0;
            canvas.gameObject.SetActive(true);
            continueText.gameObject.SetActive(false);
            dialogPanel.transform.localScale *= 0;
            _dialogState = DialogState.Opening;
            await dialogPanel.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
            await AnimateDialog(message, sender);
        }

        private async Task AnimateDialog(string message, string sender)
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
            continueText.gameObject.SetActive(true);
        }
    }
}

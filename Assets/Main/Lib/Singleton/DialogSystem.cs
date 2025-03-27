using System;
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

        private bool _isShowing = false;

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
            if(Instance._isShowing)
            {
                Debug.LogWarning("DialogSystem is already showing");
                return;
            }
            _ = Instance._ShowDialog(message, sender);
        }

        private void Update()
        {
            if (!_isShowing)
                return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _ = CloseDialog();
            }
        }

        private async UniTask CloseDialog()
        {
            _isShowing = false;
            await dialogPanel.transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
            canvas.gameObject.SetActive(false);
            senderText.text = "";
            dialogText.text = "";
            Time.timeScale = 1;
        }

        private async UniTask _ShowDialog(string message, string sender = "")
        {
            Time.timeScale = 0;
            canvas.gameObject.SetActive(true);
            dialogPanel.transform.localScale *= 0;
            await dialogPanel.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
            _isShowing = true;
            senderText.text = sender;
            var t = "";
            foreach (var c in message)
            {
                dialogText.text = t;
                t += c;
                await UniTask.WaitForSeconds(0.05f, ignoreTimeScale:true, cancellationToken:destroyCancellationToken);
            }
            continueText.gameObject.SetActive(true);
        }

        
        
    }
}

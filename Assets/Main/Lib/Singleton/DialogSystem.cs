using System.Collections;
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

        protected override void Awake()
        {
            base.Awake();
            canvas.gameObject.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            dialogButtonsContainer.SetActive(false);
            dialogText.text = "";
            senderText.text = "";
        }

        public static void ShowDialog(string message, string sender="")
        {
            _ = Instance._ShowDialog(message, sender);
        }

        private async UniTask _ShowDialog(string message, string sender = "")
        {
            canvas.gameObject.SetActive(true);
            dialogPanel.transform.localScale *= 0;
            await dialogPanel.transform.DOScale(Vector3.one, 0.5f).SetLink(Instance.gameObject).AsyncWaitForCompletion();
            senderText.text = sender;
            dialogText.text = message;
        }
        
        
    }
}

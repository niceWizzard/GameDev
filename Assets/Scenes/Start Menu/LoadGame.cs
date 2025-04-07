using Main.Lib.Save;
using Main.Lib.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Start_Menu
{
    public class LoadGame : MonoBehaviour
    {
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private Transform buttonsParent;
        private void Awake()
        {
            buttonPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            var slotChecks = SaveManager.Instance.CheckSaveSlot();
            for (var i = 0; i < slotChecks.Length; i++)
            {
                var slotExists = slotChecks[i];
                var instance = Instantiate(buttonPrefab, buttonsParent, true);
                instance.gameObject.SetActive(true);
                instance.transform.GetComponentInChildren<TMP_Text>().text = slotExists ?  $"Slot {i+1}" : "Empty";
                instance.interactable = slotExists;
                var index = i;
                instance.onClick.AddListener(() => OnNewSlotButtonClicked(slotIndex: index));
            }
        }

        private void OnNewSlotButtonClicked(int slotIndex)
        {
            SaveManager.Instance.LoadSlot(slotIndex);
            LevelLoader.Instance.LoadHub();
        }

        public void OnBackBtnClicked()
        {
            SceneManager.LoadScene("Startup");
        }  
    }
}

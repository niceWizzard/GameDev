using System;
using System.Collections.Generic;
using Main.Lib.Save;
using Main.Lib.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Start_Menu
{
    public class NewGame : MonoBehaviour
    {
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private Transform buttonsParent;
        private bool[] _slotChecks;
        private List<(TMP_Text, Button)> _slotButtons = new();

        private int _overrideIndex = -1;

        private void Awake()
        {
            buttonPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            _slotChecks = SaveManager.Instance.CheckSaveSlot();
            for (var i = 0; i < _slotChecks.Length; i++)
            {
                var slotExists = _slotChecks[i];
                var instance = Instantiate(buttonPrefab, buttonsParent, true);
                instance.gameObject.SetActive(true);
                var text = instance.transform.GetComponentInChildren<TMP_Text>();
                text.text = slotExists ? "In-Used": $"Slot {i+1}";
                var index = i;
                instance.onClick.AddListener(() => OnNewSlotButtonClicked(slotIndex: index));
                _slotButtons.Add((text, instance));
            }
        }

        private void OnNewSlotButtonClicked(int slotIndex)
        {
            var text = _slotButtons[slotIndex].Item1;
            if (_slotChecks[slotIndex] && _overrideIndex != slotIndex)
            {
                _overrideIndex = slotIndex;
                text.text = "Click again to overwrite.";
                return;
            } 
            if (_overrideIndex != -1 && slotIndex != _overrideIndex)
            {
                text.text = _slotChecks[slotIndex] ? "In-Used": $"Slot {slotIndex+1}";
            }

            if (_overrideIndex == slotIndex)
            {
                SaveManager.Instance.ClearSlot(slotIndex);
            }
            SaveManager.Instance.LoadSlot(slotIndex);
            LevelLoader.Instance.LoadHub();
        }

        public void OnBackBtnClicked()
        {
            SceneManager.LoadScene("Startup");
        }        
        
    }
}

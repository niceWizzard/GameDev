using System;
using Main.Lib.Save;
using Main.Lib.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Start_Menu
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private Button continueButton;


        private void Start()
        {
            if(SaveManager.Instance.LastSaveSlot == -1)
                continueButton.interactable = false;
        }

        public void OnQuitBtnClick()
        {
            Application.Quit();
        }

        public void OnContinueButtonClick()
        {
            SaveManager.Instance.LoadSlot(SaveManager.Instance.LastSaveSlot);
            LevelLoader.Instance.LoadHub();
        }

        public void OnLoadGameBtnClick()
        {
            SceneManager.LoadScene("Load Game");
        }

        public void OnNewGameBtnClick()
        {
            SceneManager.LoadScene("New Game");
        }

    }
}

using Main.Lib.Save;
using Main.Lib.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Startup : MonoBehaviour
    {
        public void OnQuitBtnClick()
        {
            Application.Quit();
        }

        public void OnContinueButtonClick()
        {
            SaveManager.Instance.LoadSlot(SaveManager.Instance.SaveGameData.LastSaveSlot);
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

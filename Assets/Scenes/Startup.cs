using Main.Lib.Singleton;
using UnityEngine;

namespace Scenes
{
    public class Startup : MonoBehaviour
    {
        public void OnQuitBtnClick()
        {
            Application.Quit();
        }

        public void OnStartBtnClick()
        {
            LevelLoader.Instance.LoadLevel("HubLevel");
            gameObject.SetActive(false);
        }
    }
}

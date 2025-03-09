using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

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
            Addressables.LoadSceneAsync("HubLevel");
        }
    }
}

using System;
using Main.Lib.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.UI
{
    public class PauseMenu : GameMenu
    {
        public override void Show()
        {
            base.Show();
            Time.timeScale = 0;
        }


        public override void Hide()
        {
            base.Hide();
            Time.timeScale = 1;
        }

        public void Restart()
        {
            LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().name);
            Hide();
        }

        public void Menu()
        {
            SceneManager.LoadScene("Startup");
            Hide();
        }

        public void World()
        {
            LevelLoader.Instance.LoadHub();
            Hide();
        }
        
        
    }
}

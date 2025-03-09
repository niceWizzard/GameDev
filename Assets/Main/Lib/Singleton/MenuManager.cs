using Main.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Lib.Singleton
{
    public class MenuManager : PrefabSingleton<MenuManager>
    {
        [SerializeField] private PauseMenu pauseMenu;

        protected override void Awake()
        {
            base.Awake();
            GameManager.LevelLoaded += (a) =>
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            };
        }

        private bool _paused = false;

        public void TogglePauseMenu()
        {
            _paused = !_paused;
            if(_paused)
                pauseMenu.Show();
            else
                pauseMenu.Hide();
        }
    }
}

#nullable enable
using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Main.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Singleton
{
    public enum MenuNames
    {
        PauseMenu,
        DeathMenu,
    }
    public class MenuManager : PrefabSingleton<MenuManager>
    {
        [SerializeField] private GameObject mainCanvas = null!;
        [SerializeField] private GameObject pauseMenu = null!;
        [SerializeField] private GameObject deathMenu = null!;

        private MenuNames? _currentMenu;
        private const float AnimationDuration = 0.2f;
        private TweenerCore<Vector3, Vector3, VectorOptions>? _currentTweener;
        
        
        protected override void Awake()
        {
            base.Awake();
            GameManager.LevelLoaded += (a) =>
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            };
            pauseMenu.SetActive(false);
            deathMenu.SetActive(false);
            mainCanvas.SetActive(false);
        }

        public async UniTask CloseCurrentMenu()
        {
            if (_currentTweener != null && _currentTweener.IsActive())
                return;
            if (_currentMenu == null) 
                return;
            Time.timeScale = 1;
            var obj = GetMenuObject(_currentMenu.Value);
            _currentMenu = null;
            _currentTweener = obj.transform.DOScale(Vector3.zero, AnimationDuration).SetEase(Ease.InCubic).SetUpdate(true).SetLink(gameObject);
            await UniTask.WaitForSeconds(AnimationDuration);
            obj.SetActive(false);
            mainCanvas.SetActive(false);
        }

        public void ShowMenu(MenuNames menu)
        {
            if (_currentTweener != null && _currentTweener.IsActive())
                return;
            Time.timeScale = 0;
            _currentMenu = menu;
            var obj = GetMenuObject(menu);
            mainCanvas.SetActive(true);
            obj.SetActive(true);
            obj.transform.localScale *= 0;
            _currentTweener = obj.transform.DOScale(Vector3.one, AnimationDuration).SetEase(Ease.InCubic).SetUpdate(true).SetLink(gameObject);
        }

        public void TogglePauseMenu()
        {
            if (_currentMenu == MenuNames.PauseMenu)
            {
                _ = CloseCurrentMenu();
                return;
            }
            ShowMenu(MenuNames.PauseMenu);
        }

        private GameObject GetMenuObject(MenuNames menuName)
        {
            return menuName switch
            {
                MenuNames.PauseMenu => pauseMenu,
                MenuNames.DeathMenu => deathMenu,
                _ => throw new ArgumentOutOfRangeException(nameof(menuName), menuName, null)
            };
        }

        public void ShowDeathMenu() => ShowMenu(MenuNames.DeathMenu);

        public void RetryLevel()    
        {
            LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().name);
            _ = CloseCurrentMenu();
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Startup");
            _ = CloseCurrentMenu();
        }

        public void GoToHub()
        {
            LevelLoader.Instance.LoadHub();
            _ = CloseCurrentMenu();

        }
        
    }
}
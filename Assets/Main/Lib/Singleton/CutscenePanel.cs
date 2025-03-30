using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Lib.Singleton
{
    public class CutscenePanel : PrefabSingleton<CutscenePanel>
    {
        [SerializeField] private Image panel;

        protected override void Awake()
        {
            base.Awake();
            var color = panel.color;
            color.a = 0;
            panel.color = color;
        }

        public static async UniTask ShowAsync()
        {
            var color = Instance.panel.color;
            color.a = 0;
            Instance.panel.color = color;
            await Instance.panel.DOFade(1, 0.5f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
        }

        public static async UniTask HideAsync()
        {
            DOTween.Kill(Instance.gameObject);
            var color = Instance.panel.color;
            color.a = 1;
            Instance.panel.color = color;
            await Instance.panel.DOFade(0, 0.5f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
        }

        public static void ShowImmediately()
        {
            var color = Instance.panel.color;
            color.a = 1;
            Instance.panel.color = color;
        }

        public static void HideImmediately()
        {
            var color = Instance.panel.color;
            color.a = 0;
            Instance.panel.color = color;
        }

        public static void Hide()
        {
            _ = HideAsync();
        }

        public static void Show()
        {
            _ = ShowAsync();
        }
        
        
    }
}

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

        public static async UniTask FadeInAsync()
        {
            var color = Instance.panel.color;
            color.a = 0;
            Instance.panel.color = color;
            await Instance.panel.DOFade(1, 0.25f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
        }

        public static async UniTask FadeOutAsync()
        {
            DOTween.Kill(Instance.gameObject);
            var color = Instance.panel.color;
            color.a = 1;
            Instance.panel.color = color;
            await Instance.panel.DOFade(0, 0.99f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(Instance.gameObject).AsyncWaitForCompletion();
        }

        public static void FadeOut()
        {
            _ = FadeOutAsync();
        }

        public static void FadeIn()
        {
            _ = FadeInAsync();
        }
        
        
    }
}

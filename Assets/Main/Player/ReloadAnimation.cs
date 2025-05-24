using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Player
{
    public class ReloadAnimation : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void StartAnimation(float duration)
        {
            _canvasGroup.alpha = 1;
            slider.value = 0;
            slider.maxValue = duration;
        }

        private void FixedUpdate()
        {
            if (_canvasGroup.alpha == 0)
                return;
            slider.value += Time.fixedDeltaTime;
            if (slider.value >= slider.maxValue)
            {
                _canvasGroup.alpha = 0;
            }
        }
    }
}

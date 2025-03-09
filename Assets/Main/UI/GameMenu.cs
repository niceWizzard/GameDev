using System;
using DG.Tweening;
using UnityEngine;

namespace Main.UI
{
    public abstract class GameMenu : MonoBehaviour
    {
        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        protected float AnimDuration = 0.5f;
        public virtual void Show()
        {
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, AnimDuration).SetUpdate(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            transform.DOScale(Vector3.zero, AnimDuration).SetUpdate(true);
        }
    }
}

using System;
using Main.Lib.Singleton;
using UnityEngine;

namespace Main.World
{
    public class EndingTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            LevelLoader.Instance.LoadEndingScene();
        }
    }
}

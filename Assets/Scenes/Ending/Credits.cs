using System;
using Main.Lib.Singleton;
using UnityEngine;

namespace Scenes.Ending
{
    public class Credits : MonoBehaviour
    {
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                LevelLoader.Instance.LoadMenu();
            }
        }
    }
}

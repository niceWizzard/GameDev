using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Lib
{
    [RequireComponent(typeof(Collider2D))]
    public class MobDetector : MonoBehaviour
    {
        [SerializeField] private List<MobController> excludedMobs = new();
        public event Action<MobController> OnMobEntered;
        public event Action<MobController> OnMobExited;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var mobController = other.GetComponent<MobController>();
            if (!mobController || excludedMobs.Contains(mobController))
                return;
            OnMobEntered?.Invoke(mobController);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var mobController = other.GetComponent<MobController>();
            if (!mobController || excludedMobs.Contains(mobController))
                return;
            OnMobExited?.Invoke(mobController);
        }
    }
}

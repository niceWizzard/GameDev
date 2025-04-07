using System;
using UnityEngine;

namespace Main.Lib.Level
{
    public class SurviveTime : Requirement
    {
        [SerializeField] private float requiredSeconds = 60;
        private float _activeSeconds = 0;

        public override bool CheckCompleted()
        {
            return _activeSeconds >= requiredSeconds;
        }

        private void FixedUpdate()
        {
            _activeSeconds += Time.fixedDeltaTime;
        }

        public override string GetText()
        {
            return _activeSeconds >= requiredSeconds ? "You have survived!" : $"Survive for {Mathf.RoundToInt(requiredSeconds - _activeSeconds)} seconds";
        }
    }
}
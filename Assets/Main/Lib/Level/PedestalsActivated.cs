using UnityEngine;

namespace Main.Lib.Level
{
    public class PedestalsActivated : Requirement
    {
        public override bool CheckCompleted()
        {
            var level = GameManager.CurrentLevel;
            return level.ActivatedPedestals.Count - level.TotalPedestals.Count == 0; 
        }

        public override string GetText()
        {
            var level = GameManager.CurrentLevel;
            return $"{level.ActivatedPedestals.Count}/{level.TotalPedestals.Count} Activated Pedestals";
        }
    }
}

namespace Main.Lib.Level
{
    public class WipeoutEnemies : Requirement
    {
        public override bool CheckCompleted()
        {
            return GameManager.CurrentLevel.AliveMobs <= 0;
        }

        public override string GetText()
        {
            var lvl = GameManager.CurrentLevel;
            return $"{lvl.AliveMobs}/{lvl.MobsInLevel.Count} Enemies left";
        }
    }
}
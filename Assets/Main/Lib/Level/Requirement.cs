namespace Main.Lib.Level
{
    public abstract class Requirement
    {
        public abstract bool CheckCompleted();
        public abstract string GetText();
    }

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




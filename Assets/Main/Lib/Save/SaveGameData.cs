#nullable enable
using System;
using System.Collections.Generic;

namespace Main.Lib.Save
{
    public record SaveGameData(
        HashSet<string> CompletedLevels, 
        IReadOnlyList<string> StatUpgrades,
        HashSet<string> BrokenStatues,
        HashSet<string> PlayedCutScenes
    )
    {
        public HashSet<string> CompletedLevels { get; set; } = CompletedLevels;
        public HashSet<string> BrokenStatues { get; set; } = BrokenStatues;

        public HashSet<string> PlayedCutScenes { get; set; } = PlayedCutScenes;
        public bool CompletedTutorial { get; set; } = false;

        public IReadOnlyList<string> StatUpgrades { get; set; } = StatUpgrades;
        public SaveGameData() : this(
            new HashSet<string>(),
            Array.Empty<string>(), 
            new HashSet<string>(),
            new HashSet<string>()
        ) { }
    }
}

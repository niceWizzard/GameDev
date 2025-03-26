#nullable enable
using System;
using System.Collections.Generic;

namespace Main.Lib.Save
{
    public record SaveGameData(
        HashSet<string> CompletedLevels, 
        IReadOnlyList<string> StatUpgrades,
        HashSet<string> BrokenStatues
    )
    {
        public HashSet<string> CompletedLevels { get; set; } = CompletedLevels;
        public HashSet<string> BrokenStatues { get; set; } = BrokenStatues;
        
        public IReadOnlyList<string> StatUpgrades { get; set; } = StatUpgrades;
        public SaveGameData() : this(new HashSet<string>(),Array.Empty<string>(), new HashSet<string>()) { }
    }
}

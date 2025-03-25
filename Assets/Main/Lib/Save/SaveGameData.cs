#nullable enable
using System;
using System.Collections.Generic;

namespace Main.Lib.Save
{
    public record SaveGameData(HashSet<string> CompletedLevels, IReadOnlyList<string> StatUpgrades)
    {
        public HashSet<string> CompletedLevels { get; set; } = CompletedLevels;
        
        public IReadOnlyList<string> StatUpgrades { get; set; } = StatUpgrades;
        public SaveGameData() : this(new HashSet<string>(),Array.Empty<string>()) { }
    }
}

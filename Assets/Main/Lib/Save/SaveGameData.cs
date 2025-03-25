#nullable enable
using System;
using System.Collections.Generic;

namespace Main.Lib.Save
{
    public record SaveGameData(IReadOnlyList<string> CompletedLevels, IReadOnlyList<string> StatUpgrades)
    {
        public IReadOnlyList<string> CompletedLevels { get; set; } = CompletedLevels;

        public IReadOnlyList<string> StatUpgrades { get; set; } = StatUpgrades;
        public SaveGameData() : this(Array.Empty<string>(), Array.Empty<string>()) { }
    }
}

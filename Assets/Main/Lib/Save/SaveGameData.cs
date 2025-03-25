#nullable enable
using System;
using System.Collections.Generic;

namespace Main.Lib.Save
{
    public record SaveGameData(IReadOnlyList<string> CompletedLevels)
    {
        public IReadOnlyList<string> CompletedLevels { get; set; } = CompletedLevels;
        
        public PlayerStats PlayerStats { get; set; } = new PlayerStats();
        public SaveGameData() : this(Array.Empty<string>()) { }
    }
}

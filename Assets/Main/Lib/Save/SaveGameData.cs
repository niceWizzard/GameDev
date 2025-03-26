#nullable enable
using System;
using System.Collections.Generic;

namespace Main.Lib.Save
{
    public record SaveGameData(HashSet<string> CompletedLevels)
    {
        public HashSet<string> CompletedLevels { get; set; } = CompletedLevels;
        
        public PlayerStats PlayerStats { get; set; } = new PlayerStats();
        public SaveGameData() : this(new HashSet<string>()) { }
    }
}

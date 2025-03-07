#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Main.Lib
{
    public record SaveGameData(IReadOnlyList<string> CompletedLevels)
    {
        public IReadOnlyList<string> CompletedLevels { get; set; } = CompletedLevels;

        public SaveGameData() : this(Array.Empty<string>()) { }
    }
}

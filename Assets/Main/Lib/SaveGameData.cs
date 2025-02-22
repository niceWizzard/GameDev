#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Main.Lib
{
    public class SaveStation
    {
        public string levelName = "";
        public string stationId = "";
    } 
    public record SaveGameData(IReadOnlyList<string> DefeatedEnemies, SaveStation? LastSaveStation)
    {
        public IReadOnlyList<string> DefeatedEnemies { get; set; } = DefeatedEnemies;
        public SaveStation? LastSaveStation { get; set; } = LastSaveStation;

        public SaveGameData() : this(Array.Empty<string>(), new SaveStation()) { }
    }
}

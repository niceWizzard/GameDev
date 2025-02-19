using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.Lib
{
    [Serializable]
    public class SaveGameData
    {
        public  List<string> defeatedEnemies = new List<string>();       
    }
}

using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Main.Lib.Singleton
{
    public class LevelLoader : Singleton<LevelLoader>
    {

        public void LoadLevel()
        {
            Debug.Log("LOADING!");
        }
        
        
    }
}

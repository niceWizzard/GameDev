using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Singleton
{
    public class LevelLoader : Singleton<LevelLoader>
    {

        public static void FirstLoad()
        {
            if (Instance)
            {
                // Loading -- supposed to be empty
            }
        }

        
    }
}

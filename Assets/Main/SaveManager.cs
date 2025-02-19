using System;
using System.Collections.Generic;
using System.IO;
using Main.Lib;
using Main.Lib.Singleton;
using UnityEngine;

namespace Main
{
    public class SaveManager : Singleton<SaveManager>
    {
        private string _path;
        public SaveGameData SaveGameData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _path = Path.Combine(Application.persistentDataPath ,"save_data.sav");
        }

        public void LoadSaveGameData()
        {
            if (!File.Exists(_path))
            {
                SaveGameData = new SaveGameData();
                return;
            }
            try
            {
                using var fs = new FileStream(_path, FileMode.Open);
                using var reader = new StreamReader(fs);
                var stringData = reader.ReadToEnd();
                if (string.IsNullOrEmpty(stringData))
                    throw new Exception("Invalid save data!");
                SaveGameData = JsonUtility.FromJson<SaveGameData>(stringData);
            }
            catch (Exception e)
            {
                Debug.LogError( $"Error while loading to path <{_path}>: "+ e);
                SaveGameData = new SaveGameData();
            }
        }

        public void SaveData()
        {
            try
            {
                var dataToSave = JsonUtility.ToJson(SaveGameData);
                using var fs = new FileStream(_path, FileMode.Create);
                using var writer = new StreamWriter(fs);
                writer.Write(dataToSave);
            }
            catch (Exception e)
            {
                Debug.LogError( $"Error while saving to path <{_path}>: "+ e);
                throw;
            }
        }


    }
}

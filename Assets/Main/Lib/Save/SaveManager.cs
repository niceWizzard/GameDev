using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Main.Lib.Singleton;
using Newtonsoft.Json;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Main.Lib.Save
{
    public class SaveManager : Singleton<SaveManager>
    {
        private FileHandler _fileHandler;

        private const int MaxSaveSlot = 5; 
        public int SaveSlot { get; } = 0;
        public SaveGameData SaveGameData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _fileHandler = new FileHandler(Application.persistentDataPath);
            SaveGameData = ReadSaveGameData(SaveSlot);
        }

        public bool[] CheckSaveSlot()
        {
            var saveSlotExisting = new bool[MaxSaveSlot];
            for (var i = 0; i < SaveSlot; i++)
            {
                saveSlotExisting[i] = File.Exists(GetSaveSlotPath(i));
            }
            return saveSlotExisting;
        }

        public bool SaveData(SaveGameData sgd)
        {
            var s = JsonConvert.SerializeObject(sgd);
            var result = _fileHandler.SaveFile(GetSaveSlotPath(SaveSlot), s);
            if (result)
                SaveGameData = sgd;
            return result;
        }

        public bool SaveData(Func<SaveGameData, SaveGameData> func)
        {
            return SaveData(func(SaveGameData));
        }
        
        public SaveGameData ReadSaveGameData(int saveSlot)
        {
            var path = GetSaveSlotPath(saveSlot);
            if (!_fileHandler.HasFile(path))
            {
                #if UNITY_EDITOR
                Debug.LogWarning("No save game data found.");
                #endif
                return new SaveGameData();
            }
            var dataResult = _fileHandler.LoadJsonFile<SaveGameData>(path);
            return !dataResult.IsSuccess ? new SaveGameData() : dataResult.Value;
        }

        public async UniTask<bool> SaveDataAsync(SaveGameData sgd)
        {
            var s = JsonConvert.SerializeObject(sgd);
            var result = await _fileHandler.SaveFileAsync(GetSaveSlotPath(SaveSlot), s).AttachExternalCancellation(destroyCancellationToken);
            if (result)
                SaveGameData = sgd;
            return result;
        }

        public async UniTask<bool> SaveDataAsync(Func<SaveGameData, SaveGameData> func)
        {
            return await SaveDataAsync(func(SaveGameData));
        }



        public async UniTask<SaveGameData> ReadSaveGameDataAsync(int saveSlot)
        {
            var dataResult = await _fileHandler.LoadJsonFileAsync<SaveGameData>(GetSaveSlotPath(saveSlot)).AttachExternalCancellation(destroyCancellationToken);
            return !dataResult.IsSuccess ? new SaveGameData() : dataResult.Value;
        }

        public string GetSaveSlotPath(int index)
        {
            return $"save-{index}.sav";
        }
        
        
    }
}
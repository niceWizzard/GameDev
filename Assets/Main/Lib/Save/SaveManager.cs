using System;
using System.IO;
using System.Linq;
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
        public int SaveSlot { get; private set; } = 0;
        public SaveGameData SaveGameData { get; private set; }

        public bool[] SaveSlotExists { get; private set; }
        public bool HasAnySaveFile => SaveSlotExists.Any(v => v);
        public int LastSaveSlot { get; private set; } = -1;


        protected override void Awake()
        {
            base.Awake();
            _fileHandler = new FileHandler(Application.persistentDataPath);
            LoadLastSaveSlot();
            SaveGameData = ReadSaveGameData(SaveSlot);
            SaveSlotExists = CheckSaveSlot();
        }

        private void LoadLastSaveSlot()
        {
            if (!_fileHandler.HasFile("slot.sav"))
            {
                Debug.LogWarning("Save file is missing slot.sav");
                _fileHandler.SaveFile("slot.sav", "-1");
            }
            var dataRes = _fileHandler.LoadFile("slot.sav");
            if (!dataRes.IsSuccess) return;
            if (!int.TryParse(dataRes.Value, out var data)) return;
            LastSaveSlot = data;
        }

        private void SetLastSaveSlot(int value)
        {
            _fileHandler.SaveFile("slot.sav", value.ToString());
        }

        public bool[] CheckSaveSlot()
        {
            var saveSlotExisting = new bool[MaxSaveSlot];
            for (var i = 0; i < MaxSaveSlot; i++)
            {
                saveSlotExisting[i] = File.Exists(Path.Join(Application.persistentDataPath,GetSaveSlotPath(i)));
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
            SetLastSaveSlot(saveSlot);
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
            return $"save-{index}"
            #if UNITY_EDITOR
                + "-debug"
            #endif
                + ".sav";
        }


        public void LoadSlot(int saveSlot)
        {
            SaveSlot = saveSlot;
            LoadLastSaveSlot();
            SaveGameData = ReadSaveGameData(SaveSlot);
            SaveData(SaveGameData);
        }

        public void ClearSlot(int index)
        {
            if(LastSaveSlot == index)
                SetLastSaveSlot(-1);
            File.Delete(Path.Join(Application.persistentDataPath, GetSaveSlotPath(index)));
        }
    }
}
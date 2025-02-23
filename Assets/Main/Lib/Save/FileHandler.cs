#nullable enable
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Main.Lib.Save
{
    public class FileResult<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        private FileResult(T value)
        {
            Value = value;
            IsSuccess = true;
        }

        private FileResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsSuccess = false;
        }

        public static FileResult<T> Success(T value) => new(value);
        public static FileResult<T> Failure(string errorMessage) => new(errorMessage);
    }
    public class FileHandler
    {
        private string _saveRootPath;
        public FileHandler(string saveRootPath)
        {
            _saveRootPath = saveRootPath;
        }

        public string GetPath(string path) => Path.Join(_saveRootPath, path);
        
        public FileResult<string> LoadFile(string p)
        {
            var path = GetPath(p);
            if (!File.Exists(path))
            {
                return FileResult<string>.Failure("File does not exist");
            }
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                using var reader = new StreamReader(fs);
                return FileResult<string>.Success(reader.ReadToEnd() );
            }
            catch (Exception e)
            {
                Debug.LogError( $"Error while loading to path <{path}>: "+ e);
                return FileResult<string>.Failure($"An error occured while loading file. {e.Message}");
            }
        }

        public bool SaveFile(string p, string data, bool overwrite = true)
        {
            try
            {
                var path = GetPath(p);
                if (File.Exists(path) && !overwrite)
                    return false;
                using var fs = new FileStream(path, FileMode.Create);
                using var writer = new StreamWriter(fs);
                writer.Write(data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occured while saving file. {e.Message}");
                return false;
            }
        }

        public FileResult<T> LoadJsonFile<T>(string path)
        {
            var a = LoadFile(path);
            var data = JsonConvert.DeserializeObject<T>(a.Value!);
            // var data = JsonUtility.FromJson<T>(a.Value);
            return !a.IsSuccess ? FileResult<T>.Failure(a.ErrorMessage!) : data == null ? FileResult<T>.Failure($"The file {path} data could not be read as json."):FileResult<T>.Success(data);
        }
        
        public async UniTask<FileResult<string>> LoadFileAsync(string p)
        {
            var path = GetPath(p);
            if (!File.Exists(path))
            {
                return FileResult<string>.Failure("File does not exist");
            }
            try
            {
                using var reader = new StreamReader(path);
                var content = await reader.ReadToEndAsync();
                return FileResult<string>.Success(content);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while loading file <{path}>: {e}");
                return FileResult<string>.Failure($"An error occurred while loading file. {e.Message}");
            }
        }
        
        public async UniTask<bool> SaveFileAsync(string p, string data, bool overwrite = true)
        {
            try
            {
                var path = GetPath(p);
                if (File.Exists(path) && !overwrite)
                    return false;
        
                await using var writer = new StreamWriter(path, false);
                await writer.WriteAsync(data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred while saving file. {e.Message}");
                return false;
            }
        }
        
        public async UniTask<FileResult<T>> LoadJsonFileAsync<T>(string path)
        {
            var result = await LoadFileAsync(path);
            if (!result.IsSuccess)
            {
                return FileResult<T>.Failure(result.ErrorMessage!);
            }
        
            try
            {
                var data = JsonConvert.DeserializeObject<T>(result.Value!);
                return data != null
                    ? FileResult<T>.Success(data)
                    : FileResult<T>.Failure($"The file {path} could not be read as JSON.");
            }
            catch (Exception e)
            {
                return FileResult<T>.Failure($"Failed to deserialize JSON from {path}: {e.Message}");
            }
        }
        
        
        
    }
}

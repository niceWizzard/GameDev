using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main.Lib.Singleton
{
    public abstract class PrefabSingleton<T> : Singleton<T> where T : Component
    {
        public static void InitializePrefab(string prefabPath)
        {
            // var op = Addressables.LoadAssetAsync<GameObject>(prefabKey);
            var op = Resources.Load<GameObject>(prefabPath);
            if (op == null)
            {
                Debug.LogError($"Prefab {prefabPath} not found");
                return;
            }
            Instantiate(op);
        }


    }
}

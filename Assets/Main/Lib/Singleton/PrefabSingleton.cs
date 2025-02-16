using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main.Lib.Singleton
{
    public abstract class PrefabSingleton<T> : Singleton<T> where T : Component
    {
        public static void InitializePrefab(string prefabKey)
        {
            var prefab = Addressables.LoadAssetsAsync<GameObject>(prefabKey).WaitForCompletion().First();
            Instantiate(prefab);
        }
    }
}

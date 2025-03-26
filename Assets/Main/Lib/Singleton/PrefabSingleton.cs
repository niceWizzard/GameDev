using UnityEngine;

namespace Main.Lib.Singleton
{
    public abstract class PrefabSingleton<T> : Singleton<T> where T : Component
    {
        public static void InitializePrefab(string prefabPath)
        {
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

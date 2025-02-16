using UnityEngine;

namespace Main.Lib.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        // create a private reference to T instance

        public static bool IsInitialized => Instance;

        public static T Instance { get; private set; }

        public static void Initialize()
        {
            if (Instance)
                return;
            var obj = new GameObject
            {
                name = typeof(T).Name
            };
            Instance = obj.AddComponent<T>();
        }

        protected virtual void Awake()
        {
            // create the instance
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
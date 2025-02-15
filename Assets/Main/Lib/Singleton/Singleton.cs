using UnityEngine;

namespace Main.Lib.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        // create a private reference to T instance
        private static T _instance;

        public static T Instance
        {
            get
            {
                // if instance is null
                if (_instance) return _instance;
                // find the generic instance
                _instance = FindAnyObjectByType<T>();

                // if it's null again create a new object
                // and attach the generic instance
                if (_instance) return _instance;
                var obj = new GameObject
                {
                    name = typeof(T).Name
                };
                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // create the instance
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
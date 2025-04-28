using UnityEngine;

namespace Main.Lib.Singleton
{
    public class BgMusic : PrefabSingleton<BgMusic>
    {
        [SerializeField] private AudioSource ambientSource;
        public static void PlayAmbientMusic()
        {
            Instance.ambientSource.Play();
        }

        public static void StopAll()
        {
            if(Instance.ambientSource)
                Instance.ambientSource.Stop();
        }
        
    }
}

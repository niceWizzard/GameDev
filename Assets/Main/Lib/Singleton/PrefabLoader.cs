using Main.World.Mobs.Death_Animation;
using UnityEngine;

namespace Main.Lib.Singleton
{
    public class PrefabLoader : PrefabSingleton<PrefabLoader>
    {
        [SerializeField] private DeathAnimation deathAnimationPrefab;
        
        public static DeathAnimation SpawnDeathAnimation() => Instantiate(Instance.deathAnimationPrefab);
        
    }
}

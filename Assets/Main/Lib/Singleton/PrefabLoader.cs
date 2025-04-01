using Main.World.Mobs.Death_Animation;
using Main.World.Mobs.Ghost;
using UnityEngine;

namespace Main.Lib.Singleton
{
    public class PrefabLoader : PrefabSingleton<PrefabLoader>
    {
        [SerializeField] private DeathAnimation deathAnimationPrefab;
        [SerializeField] private GhostController ghostPrefab;
        public static DeathAnimation SpawnDeathAnimation() => Instantiate(Instance.deathAnimationPrefab);

        public static GhostController SpawnGhost(Vector2 pos) => Instantiate(Instance.ghostPrefab, pos, Quaternion.identity);


    }
}

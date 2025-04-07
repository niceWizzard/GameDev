using Main.World.Mobs.Death_Animation;
using Main.World.Mobs.Ghost;
using Main.World.Mobs.Slime;
using UnityEngine;

namespace Main.Lib.Singleton
{
    public class PrefabLoader : PrefabSingleton<PrefabLoader>
    {
        [SerializeField] private DeathAnimation deathAnimationPrefab;
        [SerializeField] private GhostController ghostPrefab;
        [SerializeField] private SlimeController slimePrefab;
        public static DeathAnimation SpawnDeathAnimation() => Instantiate(Instance.deathAnimationPrefab);

        public static GhostController SpawnGhost(Vector2 pos) => Instantiate(Instance.ghostPrefab, pos, Quaternion.identity);


        public static SlimeController SpawnSlime(Vector2 position) => Instantiate(Instance.slimePrefab, position, Quaternion.identity);
        
    }
}

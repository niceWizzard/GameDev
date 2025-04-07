using System;
using System.Collections;
using System.Collections.Generic;
using Main.Lib.Mobs;
using Main.Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Main.Lib
{
    public enum MobType
    {
        Ghost,
        Slime,
    }

    [Serializable]
    public class SpawnMobInfo
    {
        public MobType mobType;
        [Range(1f,100)]
        public float spawnChance = 50f;
    }
    public abstract class MobSpawner : MonoBehaviour
    {
        [SerializeField] protected float startDelay = 0f;
        [SerializeField] protected float minSpawnDistanceFromPlayer = 3f;
        [SerializeField] protected float maxSpawnDistanceFromPlayer = 5f;
        private PlayerController _player;
        
        [SerializeField]
        protected List<SpawnMobInfo> SpawnMobInfos = new();
        
        protected Vector2 PlayerPosition => _player.Position;

        protected virtual void Start()
        {
            _player = FindAnyObjectByType<PlayerController>();
            if(!_player)
                Debug.LogError("No player controller found");
        }

        protected abstract IEnumerator Spawn();

        protected MobType GetRandomMob()
        {
            var roll = Random.Range(0,100f); 
            float cumulativeProbability = 0;
            foreach (var kvp in SpawnMobInfos)
            {
                cumulativeProbability += kvp.spawnChance; 
                if (roll <= cumulativeProbability)
                    return kvp.mobType;
            }
            return MobType.Slime;
        }

        protected Vector2 GetRandomPoint(Vector2 origin)
        {
            var iteration = 0;
            while (iteration++ < 2000)
            {
                var angle = Random.Range(0f, 360f);
                var distance = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);
                Vector2 point = (Vector3)origin + Quaternion.Euler(0, 0, angle) * Vector2.right * distance;
                if(!NavMesh.SamplePosition(point, out var navMeshHit,2, NavMesh.AllAreas))
                    continue;
                var hit = Physics2D.CircleCast(point, 1f, Vector2.zero, 0, LayerMask.GetMask("World"));
                if (hit) continue;
                return point;
            }
            Debug.LogWarning("Random position out of range, defaulting to zero.");
            return Vector2.zero;
        }
    }
}

using System;
using System.Collections;
using Main.Lib.Mobs;
using Main.Lib.Singleton;
using UnityEngine;

namespace Main.Lib
{
    public class TimedMobSpawner : MobSpawner
    {
        [SerializeField] private float timeBetweenWaves = 5;
        [SerializeField] private float timeBetweenSpawns = 0.5f;
        [SerializeField] private float mobPerWave = 5;
        
        private bool isAlive = true;

        private void OnDestroy()
        {
            isAlive = false;
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(Spawning());
        }

        private IEnumerator Spawning()
        {
            yield return new WaitForSeconds(startDelay);
            while (isAlive)
            {
                yield return Spawn();
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        
        protected override IEnumerator Spawn()
        {
            for (var i = 0; i < mobPerWave; i++)
            {
                var mobType = GetRandomMob();
                var position = GetRandomPoint(PlayerPosition);
                EnemyController enemy = mobType switch
                {
                    MobType.Ghost => PrefabLoader.SpawnGhost(position),
                    MobType.Slime => PrefabLoader.SpawnSlime(position),
                    _ => throw new Exception($"Invalid mob type received. {mobType}")
                };
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }
}
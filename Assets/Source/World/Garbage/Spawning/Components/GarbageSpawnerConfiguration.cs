using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    /// <summary>
    /// Describes the garbage spawning behaviour for this entity
    /// </summary>
    public struct GarbageSpawnerConfiguration : IComponentData
    {
        private int _spawnLimit;
        private float _spawnRate;
        private int2 _spawnCount;

        /// <summary>
        /// Determines the maximun amount of garbage instances
        /// </summary>
        /// <value></value>
        public readonly int SpawnLimit { get => _spawnLimit; }
        /// <summary>
        /// Controls the frequency in whic garbage is generated
        /// </summary>
        /// <value></value>
        public readonly float SpawnRate { get => _spawnRate; }
        /// <summary>
        /// Returns a random value between the spawn instances values
        /// </summary>
        /// <returns></returns>
        public readonly int SpawnCount { get => UnityEngine.Random.Range(_spawnCount.x, _spawnCount.y); }
        public readonly Vector2Int SpawnCountValues { get => new(_spawnCount.x, _spawnCount.y); }

        public GarbageSpawnerConfiguration(int spawnLimit, float spawnRate, Vector2Int spawnCount, Vector2 spawnRange)
        {
            _spawnLimit = spawnLimit;
            _spawnRate = spawnRate;
            _spawnCount = new int2(spawnCount.x, spawnCount.y);
        }
    }
}
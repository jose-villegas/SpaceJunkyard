using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    /// <summary>
    /// Describes the garbage spawning behaviour for this entity
    /// </summary>
    public struct GarbagePatchesSpawnerConfiguration : IComponentData
    {
        private int _spawnLimit;
        private float2 _spawnRate;
        private int2 _spawnCount;

        /// <summary>
        /// Determines the maximun amount of garbage instances
        /// </summary>
        /// <value></value>
        public readonly int SpawnLimit { get => _spawnLimit; }

        /// <summary>
        /// Controls the frequency in which garbage is generated
        /// </summary>
        /// <value></value>
        public readonly float2 SpawnRate { get => _spawnRate; }

        /// <summary>
        /// Returns a random value between the spawn instances values
        /// </summary>
        /// <returns></returns>
        public readonly int SpawnCount { get => UnityEngine.Random.Range(_spawnCount.x, _spawnCount.y); }
        public readonly Vector2Int SpawnCountValues { get => new(_spawnCount.x, _spawnCount.y); }

        public GarbagePatchesSpawnerConfiguration(int spawnLimit, Vector2 spawnRate, Vector2Int spawnCount)
        {
            _spawnLimit = spawnLimit;
            _spawnRate = new float2(spawnRate.x, spawnRate.y);
            _spawnCount = new int2(spawnCount.x, spawnCount.y);
        }
    }
}
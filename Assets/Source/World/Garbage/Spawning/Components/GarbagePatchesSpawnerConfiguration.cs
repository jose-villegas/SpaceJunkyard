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
        private readonly int2 _spawnCount;

        /// <summary>
        /// Determines the maximum amount of garbage instances
        /// </summary>
        /// <value></value>
        public int SpawnLimit { get; }

        /// <summary>
        /// Controls the frequency in which garbage is generated
        /// </summary>
        /// <value></value>
        public float2 SpawnRate { get; }

        /// <summary>
        /// Returns a random value between the spawn instances values
        /// </summary>
        /// <returns></returns>
        public readonly int SpawnCount => UnityEngine.Random.Range(_spawnCount.x, _spawnCount.y);

        public readonly Vector2Int SpawnCountValues => new(_spawnCount.x, _spawnCount.y);

        public GarbagePatchesSpawnerConfiguration(int spawnLimit, Vector2 spawnRate, Vector2Int spawnCount)
        {
            SpawnLimit = spawnLimit;
            SpawnRate = new float2(spawnRate.x, spawnRate.y);
            _spawnCount = new int2(spawnCount.x, spawnCount.y);
        }
    }
}
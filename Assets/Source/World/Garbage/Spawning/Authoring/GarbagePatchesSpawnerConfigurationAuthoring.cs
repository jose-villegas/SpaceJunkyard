using SpaceJunkyard.World.Astronomical;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    /// <summary>
    /// Handles configuration values for garbage patches that will be created
    /// surrounding this <see cref="AstronomicalBody"/>
    /// </summary>
    public class GarbagePatchesSpawnerConfigurationAuthoring : MonoBehaviour
    {
        /// <summary>
        /// Maximum amount of garbage instances per patch
        /// </summary>
        [SerializeField] private int _spawnLimit;
        /// <summary>
        /// Determines a frequency interval for spawn instances, if both values are different a random value
        /// for delay before spawn cycle is chosen between the two values
        /// </summary>
        [SerializeField] private Vector2 _spawnRate;
        /// <summary>
        /// Determines the amount of garbage instances that could appear during a spawn cycle completion. Can be random
        /// between the two values.
        /// </summary>
        [SerializeField] private Vector2Int _spawnCount;

        public int SpawnLimit { get => _spawnLimit; }
        public Vector2 SpawnRate { get => _spawnRate; }
        public Vector2Int SpawnCount { get => _spawnCount; }

        public class Baker : Baker<GarbagePatchesSpawnerConfigurationAuthoring>
        {
            public override void Bake(GarbagePatchesSpawnerConfigurationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new GarbagePatchesSpawnerConfiguration(authoring.SpawnLimit, authoring.SpawnRate,
                    authoring.SpawnCount));
            }
        }
    }
}
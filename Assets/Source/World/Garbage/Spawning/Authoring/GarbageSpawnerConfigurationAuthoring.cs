using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public class GarbageSpawnerConfigurationAuthoring : MonoBehaviour
    {
        [SerializeField] private int _spawnLimit;
        [SerializeField] private float _spawnRate;
        [SerializeField] private Vector2Int _spawnCount;
        [SerializeField] private Vector2 _spawnRange;

        public int SpawnLimit { get => _spawnLimit; }
        public float SpawnRate { get => _spawnRate; }
        public Vector2Int SpawnCount { get => _spawnCount; }
        public Vector2 SpawnRange { get => _spawnRange; }

        public class Baker : Baker<GarbageSpawnerConfigurationAuthoring>
        {
            public override void Bake(GarbageSpawnerConfigurationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new GarbageSpawnerConfiguration(authoring.SpawnLimit, authoring.SpawnRate,
                    authoring.SpawnCount, authoring.SpawnRange));
            }
        }
    }
}
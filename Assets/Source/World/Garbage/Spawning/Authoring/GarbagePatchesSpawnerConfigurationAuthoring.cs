using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public class GarbagePatchesSpawnerConfigurationAuthoring : MonoBehaviour
    {
        [SerializeField] private int _spawnLimit;
        [SerializeField] private Vector2 _spawnRate;
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
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public class GarbageSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private int _spawnLimit;
        [SerializeField] private float _spawnRate;
        [SerializeField] private int _spawnCount;
        [SerializeField] private bool _randomCount;
        [SerializeField] private Vector2 _spawnRange;

        public int SpawnLimit { get => _spawnLimit; }
        public float SpawnRate { get => _spawnRate; }
        public int SpawnCount { get => _spawnCount; }
        public bool RandomCount { get => _randomCount; }
        public Vector2 SpawnRange { get => _spawnRange; }

        public class Baker : Baker<GarbageSpawnerAuthoring>
        {
            public override void Bake(GarbageSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new GarbageSpawner(authoring.SpawnLimit, authoring.SpawnRate, 
                    authoring.SpawnCount, authoring.RandomCount, authoring.SpawnRange));
            }
        }
    }
}
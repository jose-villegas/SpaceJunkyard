using Unity.Entities;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public struct GarbageSpawnAssetReference : IComponentData
    {
        private Entity _garbagePrefab;

        public GarbageSpawnAssetReference(Entity garbagePrefab)
        {
            _garbagePrefab = garbagePrefab;
        }

        public Entity GarbagePrefab { get => _garbagePrefab; }
    }
}
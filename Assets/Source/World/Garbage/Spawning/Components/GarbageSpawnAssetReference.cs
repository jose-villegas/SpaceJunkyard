using Unity.Entities;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    /// <summary>
    /// Contains entity references to the garbage renderable asset
    /// </summary>
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
using Unity.Entities;

namespace SpaceJunkyard.Assets.Spawning
{
    /// <summary>
    /// Contains entity references to the garbage renderable asset
    /// </summary>
    public struct GameAssetReference : IComponentData
    {
        private readonly Entity _garbagePrefab;
        private readonly Entity _orbitalPatchPrefab;

        public GameAssetReference(Entity garbagePrefab, Entity orbitalPatchPrefab)
        {
            _garbagePrefab = garbagePrefab;
            _orbitalPatchPrefab = orbitalPatchPrefab;
        }

        public Entity GarbagePrefab { get => _garbagePrefab; }
        public Entity OrbitalPatchPrefab { get => _orbitalPatchPrefab; }
    }
}
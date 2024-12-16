using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbagePatch : IOrbitalPatch
    {
        private readonly Entity _orbitableBody;
        private readonly GarbagePatchesSpawnerConfiguration _garbageSpawnerConfiguration;
        private readonly float _patchSize;

        public OrbitableAreaType OrbitableAreaType => OrbitableAreaType.Gargabe;

        public Entity OrbitableBody => _orbitableBody;

        public GarbagePatchesSpawnerConfiguration GarbageSpawnerConfiguration => _garbageSpawnerConfiguration;

        public float PatchSize => _patchSize;

        public GarbagePatch(Entity orbitableBodyParent, GarbagePatchesSpawnerConfiguration garbageSpawner, float patchSize)
        {
            _orbitableBody = orbitableBodyParent;
            _garbageSpawnerConfiguration = garbageSpawner;

            _patchSize = patchSize;
        }
    }
}
using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbagePatch : IOrbitalPatch
    {
        private Entity _orbitableBody;
        private GarbagePatchesSpawnerConfiguration _garbageSpawnerConfiguration;
        private float _patchSize;

        public OrbitableAreaType OrbitableAreaType { get => OrbitableAreaType.Gargabe; }

        public Entity OrbitableBody => _orbitableBody;

        public GarbagePatchesSpawnerConfiguration GarbageSpawnerConfiguration { get => _garbageSpawnerConfiguration; }

        public float PatchSize => _patchSize;

        public GarbagePatch(Entity orbitableBodyParent, GarbagePatchesSpawnerConfiguration garbageSpawner, float patchSize)
        {
            _orbitableBody = orbitableBodyParent;
            _garbageSpawnerConfiguration = garbageSpawner;

            _patchSize = patchSize;
        }
    }
}
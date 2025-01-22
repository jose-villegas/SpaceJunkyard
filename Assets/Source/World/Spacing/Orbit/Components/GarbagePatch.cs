using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbagePatch : IOrbitalPatch
    {
        public OrbitableAreaType OrbitableAreaType => OrbitableAreaType.Gargabe;
        public Entity OrbitableBody { get; }
        public GarbagePatchesSpawnerConfiguration SpawnConfiguration { get; }
        public float PatchSize { get; }
        public bool IsActive { get; set; }
        public PatchedOrbitableAreaConfiguration PatchedOrbitConfiguration { get; }
        public int PatchIndex { get; }
        public bool IsOccupied { get; set; }

        public GarbagePatch(Entity orbitableBody, GarbagePatchesSpawnerConfiguration spawn,
            PatchedOrbitableAreaConfiguration patchedConfiguration, float patchSize,
            int patchIndex)
        {
            OrbitableBody = orbitableBody;
            SpawnConfiguration = spawn;
            PatchSize = patchSize;
            IsActive = false;
            IsOccupied = false;
            PatchIndex = patchIndex;
            PatchedOrbitConfiguration = patchedConfiguration;
        }
    }
}
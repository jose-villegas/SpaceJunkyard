using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbagePatch : IOrbitalPatch
    {
        public OrbitableAreaType OrbitableAreaType => OrbitableAreaType.Gargabe;
        public Entity Body { get; }
        public GarbagePatchesSpawnerConfiguration SpawnConfiguration { get; }
        public float PatchSize { get; }
        public bool IsActive { get; set; }
        public PatchedOrbitAreaConfigurationEntry PatchedOrbitConfiguration { get; }
        public int PatchIndex { get; }
        public bool IsOccupied { get; set; }

        public GarbagePatch(Entity body, GarbagePatchesSpawnerConfiguration spawn,
            PatchedOrbitAreaConfigurationEntry patchedConfiguration, float patchSize,
            int patchIndex)
        {
            Body = body;
            SpawnConfiguration = spawn;
            PatchSize = patchSize;
            IsActive = false;
            IsOccupied = false;
            PatchIndex = patchIndex;
            PatchedOrbitConfiguration = patchedConfiguration;
        }
    }
}
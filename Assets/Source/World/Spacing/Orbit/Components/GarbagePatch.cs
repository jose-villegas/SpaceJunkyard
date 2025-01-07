using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbagePatch : IOrbitalPatch
    {
        private readonly Entity _orbitableBody;
        private readonly GarbagePatchesSpawnerConfiguration _spawnConfiguration;
        private readonly PatchedOrbitableAreaConfiguration _patchedOrbitConfiguration;
        private readonly float _patchSize;
        private bool _isActive;
        private bool _isOccupied;
        private int _patchIndex;

        public OrbitableAreaType OrbitableAreaType => OrbitableAreaType.Gargabe;

        public Entity OrbitableBody => _orbitableBody;

        public GarbagePatchesSpawnerConfiguration SpawnConfiguration => _spawnConfiguration;

        public float PatchSize => _patchSize;

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public PatchedOrbitableAreaConfiguration PatchedOrbitConfiguration => _patchedOrbitConfiguration;

        public int PatchIndex => _patchIndex;

        public bool IsOccupied
        {
            get => _isOccupied;
            set => _isOccupied = value;
        }

        public GarbagePatch(Entity orbitableBody, GarbagePatchesSpawnerConfiguration spawn,
            PatchedOrbitableAreaConfiguration patchedConfiguration, float patchSize,
            int patchIndex)
        {
            _orbitableBody = orbitableBody;
            _spawnConfiguration = spawn;
            _patchSize = patchSize;
            _isActive = false;
            _isOccupied = false;
            _patchIndex = patchIndex;
            _patchedOrbitConfiguration = patchedConfiguration;
        }
    }
}
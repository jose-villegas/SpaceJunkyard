using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbagePatch : IOrbitalPatch
    {
        private Entity _orbitableBody;
        private GarbageSpawnerConfiguration _garbageSpawnerConfiguration;
        private GarbageSpawnControl _garbageControl;
        private float _patchSize;

        public OrbitableAreaType OrbitableAreaType { get => OrbitableAreaType.Gargabe; }

        public Entity OrbitableBody => _orbitableBody;

        public GarbageSpawnerConfiguration GarbageSpawnerConfiguration { get => _garbageSpawnerConfiguration; }

        public float PatchSize => _patchSize;

        public GarbagePatch(Entity orbitableBodyParent, GarbageSpawnerConfiguration garbageSpawner, float patchSize)
        {
            _orbitableBody = orbitableBodyParent;
            _garbageSpawnerConfiguration = garbageSpawner;

            _garbageControl = new(garbageSpawner);
            _patchSize = patchSize;
        }

        public bool CanSpawn(double elapsedTime)
        {
            if (_garbageControl.IsOverflowed) return false;

            var currentTime = elapsedTime;
            var timeSpan = currentTime - _garbageControl.CurrentTick;

            if (timeSpan < _garbageControl.SpawnRate) return false;

            return true;
        }

        public void SpawnCheckTick(double elapsedTime)
        {
            _garbageControl.CurrentTick = elapsedTime;
        }

        public int CurrentGarbageCount { get => _garbageControl.CurrentGarbageCount; set => _garbageControl.CurrentGarbageCount = value; }
    }
}
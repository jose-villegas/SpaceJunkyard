using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbageSpawnControl
    {
        // Control fields
        private int _currentGarbageCount;
        private double _currentTick;

        public readonly bool IsOverflowed => _currentGarbageCount >= _spawnLimit;

        public int CurrentGarbageCount { get => _currentGarbageCount; set => _currentGarbageCount = value; }
        public double CurrentTick { get => _currentTick; set => _currentTick = value; }
        public float SpawnRate { get => _spawnRate; }

        private float _spawnRate;
        private int _spawnLimit;

        public GarbageSpawnControl(float spawnRate, int spawnLimit)
        {
            _spawnRate = spawnRate;
            _spawnLimit = spawnLimit;

            _currentGarbageCount = 0;
            _currentTick = 0f;
        }

        public GarbageSpawnControl(GarbageSpawnerConfiguration configuration) : this(configuration.SpawnRate, configuration.SpawnLimit) { }
    }

    public struct GarbagePatch : IOrbitalPatch
    {
        private Entity _orbitableBody;
        private GarbageSpawnerConfiguration _garbageSpawnerConfiguration;
        private GarbageSpawnControl _garbageControl;

        public OrbitableAreaType OrbitableAreaType { get => OrbitableAreaType.Gargabe; }

        public Entity OrbitableBody => _orbitableBody;

        public GarbageSpawnerConfiguration GarbageSpawnerConfiguration { get => _garbageSpawnerConfiguration; }

        public GarbagePatch(Entity orbitableBodyParent, GarbageSpawnerConfiguration garbageSpawner)
        {
            _orbitableBody = orbitableBodyParent;
            _garbageSpawnerConfiguration = garbageSpawner;

            _garbageControl = new(garbageSpawner);
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
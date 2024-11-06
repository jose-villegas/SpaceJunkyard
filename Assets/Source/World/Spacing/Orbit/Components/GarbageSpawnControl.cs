using SpaceJunkyard.World.Garbage.Spawning;

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
}
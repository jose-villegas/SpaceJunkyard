using SpaceJunkyard.World.Garbage.Spawning;

namespace SpaceJunkyard.World.Spacing
{
    public struct GarbageSpawnControl
    {
        // Control fields
        private int _currentGarbageCount;
        private double _currentTick;
        private double _nextInstanceIn;

        /// <summary>
        /// Current amount of garbage instances, to check against <see cref="GarbagePatchesSpawnerConfiguration.SpawnLimit"/>
        /// </summary>
        public int CurrentGarbageCount { get => _currentGarbageCount; set => _currentGarbageCount = value; }

        /// <summary>
        /// The time tick value of this control data
        /// </summary>
        public double CurrentTick { get => _currentTick; set => _currentTick = value; }

        /// <summary>
        /// The time for the next instanciation in seconds
        /// </summary>
        public double NextInstanceIn { get => _nextInstanceIn; set => _nextInstanceIn = value; }
    }
}
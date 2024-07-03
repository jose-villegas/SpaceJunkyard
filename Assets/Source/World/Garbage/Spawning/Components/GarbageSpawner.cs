using System;
using Unity.Entities;
using UnityEngine.AI;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public struct GarbageSpawner : IComponentData
    {
        private int _spawnLimit;
        private float _spawnRate;
        private int _spawnCount;
        private bool _randomCount;

        public int SpawnLimit { get => _spawnLimit; }
        public float SpawnRate { get => IsOverflow ? 0 : _spawnRate; }
        public int SpawnCount { get => IsOverflow ? 0 : (RandomCount ? UnityEngine.Random.Range(1, _spawnCount) : _spawnCount); }
        public bool RandomCount { get => _randomCount; }

        private int _currentGarbageCount;
        private DateTime _currentTick;
        public int CurrentGarbageCount { get => _currentGarbageCount; set => _currentGarbageCount = value; }
        public DateTime CurrentTick { get => _currentTick; set => _currentTick = value; }

        public bool CanSpawn()
        {
            if (IsOverflow) return false;

            var currentTime = DateTime.Now;
            var timeSpan = currentTime - _currentTick;

            if (timeSpan.TotalSeconds < SpawnRate) return false;

            return true;
        }

        public void SpawnCheckTick()
        {
            _currentTick = DateTime.Now;
        }


        public GarbageSpawner(int spawnLimit, float spawnRate, int spawnCount, bool randomCount)
        {
            _spawnLimit = spawnLimit;
            _spawnRate = spawnRate;
            _spawnCount = spawnCount;
            _randomCount = randomCount;
            _currentGarbageCount = 0;
            _currentTick = DateTime.Now;
        }

        public readonly bool IsOverflow => _currentGarbageCount >= _spawnLimit;

    }
}
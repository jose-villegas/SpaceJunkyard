using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    /// <summary>
    /// Describes the garbage spawning behaviour for this entity
    /// </summary>
    public struct GarbageSpawner : IComponentData
    {
        private int _spawnLimit;
        private float _spawnRate;
        private int _spawnCount;
        private bool _randomCount;
        private float2 _spawnRange;

        public int SpawnLimit { get => _spawnLimit; }
        public float SpawnRate { get => IsOverflow ? 0 : _spawnRate; }
        public int SpawnCount { get => IsOverflow ? 0 : (RandomCount ? UnityEngine.Random.Range(1, _spawnCount) : _spawnCount); }
        public bool RandomCount { get => _randomCount; }
        public Vector2 SpawnRange { get => _spawnRange; }

        private int _currentGarbageCount;
        private double _currentTick;
        public int CurrentGarbageCount { get => _currentGarbageCount; set => _currentGarbageCount = value; }
        public double CurrentTick { get => _currentTick; set => _currentTick = value; }

        public bool CanSpawn(double elapsedTime)
        {
            if (IsOverflow) return false;

            var currentTime = elapsedTime;
            var timeSpan = currentTime - _currentTick;

            if (timeSpan < SpawnRate) return false;

            return true;
        }

        public void SpawnCheckTick(double elapsedTime)
        {
            _currentTick = elapsedTime;
        }


        public GarbageSpawner(int spawnLimit, float spawnRate, int spawnCount, bool randomCount, Vector2 spawnRange)
        {
            _spawnLimit = spawnLimit;
            _spawnRate = spawnRate;
            _spawnCount = spawnCount;
            _randomCount = randomCount;
            _spawnRange = spawnRange;

            _currentGarbageCount = 0;
            _currentTick = 0;
        }

        public readonly bool IsOverflow => _currentGarbageCount >= _spawnLimit;

    }
}
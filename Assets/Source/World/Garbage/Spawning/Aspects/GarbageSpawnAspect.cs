using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public readonly partial struct GarbageSpawnAspect : IAspect
    {
        public readonly RefRW<GarbageSpawner> garbageSpawner;
        public readonly RefRO<AstronomicalBody> astronomicalBody;
        public readonly RefRO<LocalTransform> localTransform;

        [BurstCompile]
        public void SpawnGarbage(double elapsedTime, ref EntityCommandBuffer entityCommandBuffer, ref GarbageSpawnAssetReference assetReference)
        {
            if (!garbageSpawner.ValueRO.CanSpawn(elapsedTime)) return;

            var spawnCount = garbageSpawner.ValueRO.SpawnCount;

            for (var i = 0; i < spawnCount; i++)
            {
                var spawn = entityCommandBuffer.Instantiate(assetReference.GarbagePrefab);

                // add orbit info
                var spawnRange = garbageSpawner.ValueRO.SpawnRange;
                var spawnPlace = Random.Range(spawnRange.x, spawnRange.y);
                var angle = Random.Range(0f, (float)(2f * Constants.PI));
                entityCommandBuffer.AddComponent(spawn, new Orbiter(spawnPlace, spawnPlace, angle));
                // add shared orbitable point
                entityCommandBuffer.AddComponent(spawn, new OrbiterPoint(astronomicalBody.ValueRO));
            }

            garbageSpawner.ValueRW.CurrentGarbageCount += spawnCount;
            // set timer
            garbageSpawner.ValueRW.SpawnCheckTick(elapsedTime);
        }
    }
}
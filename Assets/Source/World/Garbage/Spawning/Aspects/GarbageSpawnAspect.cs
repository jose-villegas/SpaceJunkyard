using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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
                var spawnPlace = UnityEngine.Random.Range(spawnRange.x, spawnRange.y);
                var angle = UnityEngine.Random.Range(0f, (float)(2f * Constants.PI));
                
                // add components
                var orbiter = new Orbiter(spawnPlace, spawnPlace, angle);
                var orbiterPoint = new OrbiterPoint(astronomicalBody.ValueRO);
                entityCommandBuffer.AddComponent(spawn, orbiter);
                entityCommandBuffer.AddComponent(spawn, orbiterPoint);
                
                // calculate initial position
                var position = orbiter.CalculateCurrentEllipticalPosition(orbiterPoint.Body.GravityCenter);
                entityCommandBuffer.SetComponent(spawn, LocalTransform.FromPositionRotationScale(position, quaternion.identity, 0.2f));
            }

            garbageSpawner.ValueRW.CurrentGarbageCount += spawnCount;
            // set timer
            garbageSpawner.ValueRW.SpawnCheckTick(elapsedTime);
        }
    }
}
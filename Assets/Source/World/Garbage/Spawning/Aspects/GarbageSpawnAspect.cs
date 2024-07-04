using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public readonly partial struct GarbageSpawnAspect : IAspect
    {
        public readonly RefRW<GarbageSpawner> garbageSpawner;
        public readonly RefRO<AstronomicalBody> astronomicalBody;

        public void SpawnGarbage(double elapsedTime, EntityCommandBuffer entityCommandBuffer, GarbageSpawnAssetReference assetReference)
        {
            if (!garbageSpawner.ValueRO.CanSpawn(elapsedTime)) return;

            var spawnCount = garbageSpawner.ValueRO.SpawnCount;

            for (var i = 0; i < spawnCount; i++)
            {
                var spawn = entityCommandBuffer.Instantiate(assetReference.GarbagePrefab);
                var bodyData = new OrbitData(astronomicalBody.ValueRO.Name, Random.Range(2.5f, 8f));

                entityCommandBuffer.AddComponent(spawn, new Orbiter(bodyData, Random.Range(0f, 360f)));
            }

            garbageSpawner.ValueRW.CurrentGarbageCount += spawnCount;
            // set timer
            garbageSpawner.ValueRW.SpawnCheckTick(elapsedTime);
        }
    }
}
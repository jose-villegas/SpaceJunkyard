using SpaceJunkyard.Assets.Spawning;
using SpaceJunkyard.World.Spacing;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    [BurstCompile]
    public readonly partial struct GarbageSpawnAspect : IAspect
    {
        public readonly RefRW<GarbagePatch> garbagePatch;
        public readonly RefRO<LocalTransform> localTransform;
        public readonly Entity entity;

        [BurstCompile]
        public void SpawnGarbage(double elapsedTime, ref EntityCommandBuffer entityCommandBuffer, ref GameAssetReference assetReference)
        {
            var spawnConfiguration = garbagePatch.ValueRO.GarbageSpawnerConfiguration;

            if (!garbagePatch.ValueRW.CanSpawn(elapsedTime)) return;

            var instancesToSpawn = spawnConfiguration.SpawnCount;

            for (var i = 0; i < instancesToSpawn; i++)
            {
                var spawn = entityCommandBuffer.Instantiate(assetReference.GarbagePrefab);

                // add orbit info
                //var spawnRange = garbageSpawner.SpawnRange;
                //var spawnPlace = UnityEngine.Random.Range(spawnRange.x, spawnRange.y);
                //var angle = UnityEngine.Random.Range(0f, (float)(2f * Constants.PI));

                // add components
                // var orbiter = new Orbiter(spawnPlace, spawnPlace, angle);
                //var orbiterPoint = new OrbiterPoint(astronomicalBody.ValueRO);
                //entityCommandBuffer.AddComponent(spawn, orbiter);
                //entityCommandBuffer.AddComponent(spawn, orbiterPoint);

                // calculate initial position
                //var position = localTransform.ValueRO.Position;
                //entityCommandBuffer.SetComponent(spawn, LocalTransform.FromPositionRotationScale(position, quaternion.identity, 0.2f));

                // parent the garbage spawn
                entityCommandBuffer.AddComponent<Parent>(spawn);
                entityCommandBuffer.SetComponent(spawn, new Parent() { Value = entity });
            }

            garbagePatch.ValueRW.CurrentGarbageCount += instancesToSpawn;
            // set timer
            garbagePatch.ValueRW.SpawnCheckTick(elapsedTime);
        }
    }
}
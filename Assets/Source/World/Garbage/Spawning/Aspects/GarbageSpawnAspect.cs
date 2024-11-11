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
        public void SpawnGarbage(ref EntityCommandBuffer entityCommandBuffer, ref GameAssetReference assetReference)
        {
            var patchSize = garbagePatch.ValueRO.PatchSize;
            var spawnConfiguration = garbagePatch.ValueRO.GarbageSpawnerConfiguration;
            var instancesToSpawn = spawnConfiguration.SpawnCount;

            for (var i = 0; i < instancesToSpawn; i++)
            {
                var spawn = entityCommandBuffer.Instantiate(assetReference.GarbagePrefab);

                // create random point within an unit square
                var point = new float3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
                // normalize to convert to a unit circle
                point = math.normalize(point) * UnityEngine.Random.Range(-1f, 1f) * patchSize / 2f;

                // calculate initial 
                var transform = LocalTransform.FromPosition(point);
                entityCommandBuffer.SetComponent(spawn, transform);

                // parent the garbage spawn
                entityCommandBuffer.AddComponent<Parent>(spawn);
                entityCommandBuffer.SetComponent(spawn, new Parent() { Value = entity });
            }
        }
    }
}
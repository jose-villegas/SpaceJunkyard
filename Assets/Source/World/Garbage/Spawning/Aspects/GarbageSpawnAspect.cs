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
        public readonly Entity entity;

        [BurstCompile]
        public void SpawnGarbage(ref EntityCommandBuffer entityCommandBuffer, ref GameAssetReference assetReference, 
            in int instancesToSpawn)
        {
            var patchSize = garbagePatch.ValueRO.PatchSize;

            for (var i = 0; i < instancesToSpawn; i++)
            {
                var spawn = entityCommandBuffer.Instantiate(assetReference.GarbagePrefab);

                // create random point within a unit square
                var point = float3.zero;
                var unit = UnityEngine.Random.insideUnitCircle;
                point.x = unit.x; point.z = unit.y;
                // match to patch size distance as radius
                point = point * patchSize / 2f;

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
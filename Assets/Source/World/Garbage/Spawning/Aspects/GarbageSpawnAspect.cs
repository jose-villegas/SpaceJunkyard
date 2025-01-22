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
        private readonly RefRO<GarbagePatch> _garbagePatch;
        private readonly Entity _entity;

        public GarbagePatch Patch => _garbagePatch.ValueRO;

        [BurstCompile]
        public void SpawnGarbage(ref EntityCommandBuffer entityCommandBuffer, ref GameAssetReference assetReference, 
            in int instancesToSpawn)
        {
            var patchSize = _garbagePatch.ValueRO.PatchSize;

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
                entityCommandBuffer.AddComponent(spawn, new Parent { Value = _entity });
                
                // add to buffer, keep reference of garbage instances
                entityCommandBuffer.AppendToBuffer(_entity, new GarbageInstanceEntry(spawn));
            }
        }
    }
}
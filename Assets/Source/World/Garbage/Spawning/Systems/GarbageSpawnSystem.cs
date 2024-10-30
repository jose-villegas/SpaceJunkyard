using SpaceJunkyard.Assets.Spawning;
using SpaceJunkyard.World.Spacing;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct GarbageSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<GameAssetReference>().Build();
            var spawners = SystemAPI.QueryBuilder().WithAll<GarbagePatch>().Build();

            state.RequireAnyForUpdate(configuration, spawners);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var assetReference = SystemAPI.GetSingleton<GameAssetReference>();
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            var elapsedTime = SystemAPI.Time.ElapsedTime;

            foreach (var aspect in SystemAPI.Query<GarbageSpawnAspect>())
            {
                aspect.SpawnGarbage(elapsedTime, ref entityCommandBuffer, ref assetReference);
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
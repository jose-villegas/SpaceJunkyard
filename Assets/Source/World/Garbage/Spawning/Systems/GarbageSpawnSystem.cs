using SpaceJunkyard.Assets.Spawning;
using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
using SpaceJunkyard.World.Spacing;
using Unity.Burst;
using Unity.Entities;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    [UpdateAfter(typeof(OrbitingSystem))]
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
            //return;

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
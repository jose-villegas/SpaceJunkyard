using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
using Unity.Burst;
using Unity.Entities;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    [UpdateAfter(typeof(OrbitingSystem))]
    public partial struct GarbageSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<GarbageSpawnAssetReference>().Build();
            var spawners = SystemAPI.QueryBuilder().WithAll<GarbageSpawner, AstronomicalBody, Orbitable>().Build();

            state.RequireAnyForUpdate(configuration, spawners);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var assetReference = SystemAPI.GetSingleton<GarbageSpawnAssetReference>();
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
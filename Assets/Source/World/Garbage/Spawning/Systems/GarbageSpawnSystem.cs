using SpaceJunkyard.World.Dynamics.Orbiting;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public partial struct GarbageSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<GarbageSpawnAssetReference>().Build();
            var spawners = SystemAPI.QueryBuilder().WithAll<GarbageSpawner>().Build();

            state.RequireAnyForUpdate(configuration, spawners);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var assetReference = SystemAPI.GetSingleton<GarbageSpawnAssetReference>();
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach ((var garbageSpawner, var localTransform) in SystemAPI.Query<RefRW<GarbageSpawner>, RefRO<LocalTransform>>())
            {
                if (!garbageSpawner.ValueRO.CanSpawn()) continue;

                var spawnCount = garbageSpawner.ValueRO.SpawnCount;

                for (var i = 0; i < spawnCount; i++)
                {
                    var spawn = entityCommandBuffer.Instantiate(assetReference.GarbagePrefab);
                    var bodyData = new BodyData(localTransform.ValueRO.Position, Random.Range(2.5f, 8f));

                    entityCommandBuffer.AddComponent(spawn, new Orbiter(bodyData, Random.Range(0f, 360f)));
                }

                garbageSpawner.ValueRW.CurrentGarbageCount += spawnCount;
                // set timer
                garbageSpawner.ValueRW.SpawnCheckTick();
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
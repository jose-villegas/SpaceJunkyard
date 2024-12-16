using SpaceJunkyard.Assets.Spawning;
using SpaceJunkyard.World.Spacing;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct GarbageSpawnSystem : ISystem
    {
        private NativeHashMap<Entity, GarbageSpawnControl> _garbageControl;

        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<GameAssetReference>().Build();
            var spawners = SystemAPI.QueryBuilder().WithAll<GarbagePatch>().Build();

            state.RequireAnyForUpdate(configuration, spawners);

            // initialize control map
            _garbageControl =
                new NativeHashMap<Entity, GarbageSpawnControl>(spawners.CalculateEntityCount(), Allocator.Persistent);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var assetReference = SystemAPI.GetSingleton<GameAssetReference>();
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var elapsedTime = SystemAPI.Time.ElapsedTime;

            foreach ((var aspect, var entity) in SystemAPI.Query<GarbageSpawnAspect>().WithEntityAccess())
            {
                var garbagePatch = aspect.Patch;
                var spawnConfig = garbagePatch.GarbageSpawnerConfiguration;

                // if we haven't spawned for this patch, schedule for spawn
                if (!_garbageControl.TryGetValue(entity, out var control))
                {
                    // choose random starting delay for frequency
                    var delay = UnityEngine.Random.Range(spawnConfig.SpawnRate.x, spawnConfig.SpawnRate.y);
                    // setup control data
                    control = new GarbageSpawnControl() {NextInstanceIn = delay, CurrentTick = elapsedTime};
                    _garbageControl.Add(entity, control);
                }

                // limit of garbage instances reached
                if (control.CurrentGarbageCount >= spawnConfig.SpawnLimit)
                {
                    continue;
                }

                // calculate if we are within the time frame to instance
                var timeSpan = elapsedTime - control.CurrentTick;

                if (timeSpan < control.NextInstanceIn) continue;

                // choose a number of instances to spawn
                var instanceCount = spawnConfig.SpawnCount;

                if (instanceCount + control.CurrentGarbageCount > spawnConfig.SpawnLimit)
                {
                    instanceCount = spawnConfig.SpawnLimit - control.CurrentGarbageCount;
                }

                // finally instance the garbage prefabs
                aspect.SpawnGarbage(ref entityCommandBuffer, ref assetReference, instanceCount);
                // update control values
                control.CurrentTick = elapsedTime;
                control.CurrentGarbageCount += instanceCount;
                _garbageControl[entity] = control;
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
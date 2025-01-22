using SpaceJunkyard.Assets.Spawning;
using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
using SpaceJunkyard.World.Garbage.Spawning;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SpaceJunkyard.World.Spacing
{
    /// <summary>
    /// Takes cares of instancing orbital patch prefabs if requested by an <see cref="AstronomicalBody"/>
    /// </summary>
    public partial struct OrbitalPatchInstancingSystem : ISystem
    {
        private ComponentLookup<GarbagePatchesSpawnerConfiguration> _garbageSpawnerLookup;

        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, PatchedOrbitAreaConfigurationEntry>().Build();
            _garbageSpawnerLookup = state.GetComponentLookup<GarbagePatchesSpawnerConfiguration>(true);

            state.RequireForUpdate<GameAssetReference>();
            state.RequireForUpdate(configuration);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _garbageSpawnerLookup.Update(ref state);
            var assetReference = SystemAPI.GetSingleton<GameAssetReference>();
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach ((var request, var body, var entity) in SystemAPI
                         .Query<DynamicBuffer<PatchedOrbitAreaConfigurationEntry>, RefRO<AstronomicalBody>>().WithEntityAccess())
            {
                for (int i = 0; i < request.Length; i++)
                {
                    var entry = request[i];

                    if (entry.OrbitableAreaType == OrbitableAreaType.Gargabe)
                    {
                        InstanceGarbagePatches(ref assetReference, ref entityCommandBuffer, body, entity,
                            entry, _garbageSpawnerLookup.GetRefRO(entity));
                    }
                }

                // remove request
                entityCommandBuffer.RemoveComponent<PatchedOrbitAreaConfigurationEntry>(entity);
                // create data structures for garbage patch activation
                entityCommandBuffer.AddComponent<RequestGarbagePatchActivatorCheck>(entity);
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }

        private void InstanceGarbagePatches
        (
            ref GameAssetReference assetReference,
            ref EntityCommandBuffer entityCommandBuffer,
            RefRO<AstronomicalBody> body,
            Entity bodyEntity,
            PatchedOrbitAreaConfigurationEntry patchConfigurationEntry,
            RefRO<GarbagePatchesSpawnerConfiguration> spawnerConfiguration
        )
        {
            var orbitRadius = patchConfigurationEntry.CenterHeight;
            var orbitDiameter = 2f * orbitRadius;

            // calculate each patch radius
            var patchSize = math.PI * (orbitDiameter / patchConfigurationEntry.PatchCount);
            var angleSeparation = math.PI2 / patchConfigurationEntry.PatchCount;

            // create garbage patches circling the center height orbit
            for (var i = 0; i < patchConfigurationEntry.PatchCount; i++)
            {
                // cycle unit circle coordinates
                var newPatch = entityCommandBuffer.Instantiate(assetReference.OrbitalPatchPrefab);

                // add orbiting properties
                var orbiter = new Orbiter(orbitRadius, angleSeparation * i);
                var orbiterPoint = new OrbiterPoint(body.ValueRO);
                entityCommandBuffer.AddComponent(newPatch, orbiter);
                entityCommandBuffer.AddComponent(newPatch, orbiterPoint);

                // calculate initial position
                var position = orbiter.CalculateCurrentEllipticalPosition(orbiterPoint.Body.GravityCenter);
                entityCommandBuffer.SetComponent(newPatch,
                    LocalTransform.FromPositionRotationScale(position, quaternion.identity, 1f));

                // add parent
                entityCommandBuffer.AddComponent<Parent>(newPatch);
                entityCommandBuffer.SetComponent(newPatch, new Parent {Value = patchConfigurationEntry.Container});

                // identify as orbital patch entity
                entityCommandBuffer.AddComponent(newPatch,
                    new GarbagePatch(bodyEntity, spawnerConfiguration.ValueRO, patchConfigurationEntry,
                        patchSize, i));
                entityCommandBuffer.AddBuffer<GarbageInstanceEntry>(newPatch);
                
                // save reference to the astronomical body
                var buffer = entityCommandBuffer.AddBuffer<OrbitalAreaBufferEntry>(bodyEntity);
                buffer.Add(new OrbitalAreaBufferEntry(OrbitableAreaType.Gargabe, newPatch));
            }
        }
    }
}
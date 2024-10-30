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
    public partial struct OrbitalPatchCreatorSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<RequestOrbitableSpacePatches, AstronomicalBody>().Build();
            state.RequireForUpdate(configuration);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var assetReference = SystemAPI.GetSingleton<GameAssetReference>();
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            var garbageSpawnerLookup = state.GetComponentLookup<GarbageSpawnerConfiguration>(true);

            foreach ((var request, var body, var entity) in SystemAPI.Query<RefRO<RequestOrbitableSpacePatches>, RefRO<AstronomicalBody>>().WithEntityAccess())
            {
                var requestRO = request.ValueRO;

                var garbagePatchConfiguration = requestRO.GarbageAreaConfiguration;
                InstanceGarbagePatches(ref assetReference, ref entityCommandBuffer, body, garbagePatchConfiguration, entity, garbageSpawnerLookup.GetRefRO(entity));

                // remove request
                entityCommandBuffer.RemoveComponent<RequestOrbitableSpacePatches>(entity);
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }

        private void InstanceGarbagePatches
        (
            ref GameAssetReference assetReference,
            ref EntityCommandBuffer entityCommandBuffer,
            RefRO<AstronomicalBody> body,
            PatchedOrbitableAreaConfiguration garbagePatchConfiguration,
            Entity orbitingBodyEntity,
            RefRO<GarbageSpawnerConfiguration> garbageSpawnerConfiguration
        )
        {
            var orbitRadius = garbagePatchConfiguration.CenterHeight;
            var orbitDiameter = 2f * orbitRadius;

            // calculate each patch radius
            var patchScale = math.PI * (orbitDiameter / garbagePatchConfiguration.PatchCount);
            var angleSeparation = math.PI2 / garbagePatchConfiguration.PatchCount;

            // create garbage patches circling the center height orbit
            for (var i = 0; i < garbagePatchConfiguration.PatchCount; i++)
            {
                // cycle unit circle coordinates
                var newPatch = entityCommandBuffer.Instantiate(assetReference.OrbitalPatchPrefab);

                // add orbiting properties
                var orbiter = new Orbiter(orbitRadius, orbitRadius, angleSeparation * i);
                var orbiterPoint = new OrbiterPoint(body.ValueRO);
                entityCommandBuffer.AddComponent(newPatch, orbiter);
                entityCommandBuffer.AddComponent(newPatch, orbiterPoint);

                // calculate initial position
                var position = orbiter.CalculateCurrentEllipticalPosition(orbiterPoint.Body.GravityCenter);
                entityCommandBuffer.SetComponent(newPatch, LocalTransform.FromPositionRotationScale(position, quaternion.identity, patchScale));

                // add parent
                entityCommandBuffer.AddComponent<Parent>(newPatch);
                entityCommandBuffer.SetComponent(newPatch, new Parent() { Value =  garbagePatchConfiguration.Container});

                // identify as orbital patch entity
                entityCommandBuffer.AddComponent(newPatch, new GarbagePatch(orbitingBodyEntity, garbageSpawnerConfiguration.ValueRO));
            }
        }
    }
}
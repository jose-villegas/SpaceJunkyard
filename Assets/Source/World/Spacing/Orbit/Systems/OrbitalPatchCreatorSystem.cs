using SpaceJunkyard.Assets.Spawning;
using SpaceJunkyard.World.Astronomical;
using SpaceJunkyard.World.Dynamics.Orbiting;
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


        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var assetReference = SystemAPI.GetSingleton<GameAssetReference>();
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            foreach ((_, var body, var entity) in SystemAPI.Query<RefRO<RequestOrbitableSpacePatches>, RefRO<AstronomicalBody>>().WithEntityAccess())
            {
                // calculate how many patches we need to cover the diameter
                var newPatch = entityCommandBuffer.Instantiate(assetReference.OrbitalPatchPrefab);

                // add orbiting properties
                var orbiter = new Orbiter(15, 15, 0);
                var orbiterPoint = new OrbiterPoint(body.ValueRO);
                entityCommandBuffer.AddComponent(newPatch, orbiter);
                entityCommandBuffer.AddComponent(newPatch, orbiterPoint);

                // calculate initial position
                var position = orbiter.CalculateCurrentEllipticalPosition(orbiterPoint.Body.GravityCenter);
                entityCommandBuffer.SetComponent(newPatch, LocalTransform.FromPositionRotationScale(position, quaternion.identity, 5f));

                // identify as orbital patch entity
                entityCommandBuffer.AddComponent(newPatch, new OrbitalPatch());

                // remove request
                entityCommandBuffer.RemoveComponent<RequestOrbitableSpacePatches>(entity);
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
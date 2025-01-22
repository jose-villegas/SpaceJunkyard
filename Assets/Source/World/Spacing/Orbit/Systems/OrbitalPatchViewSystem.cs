using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace SpaceJunkyard.World.Spacing
{
    /// <summary>
    /// Orbital patches spawn barebones, this system takes 
    /// cares of properly setting values for the intended view
    /// </summary>
    public partial struct OrbitalPatchViewSystem : ISystem
    {
        private ComponentLookup<GarbagePatch> _garbagePatchLookUp;
        private ComponentLookup<LocalTransform> _localTransformLookUp;

        public void OnCreate(ref SystemState state)
        {
            var configuration = SystemAPI.QueryBuilder().WithAll<ConfigureOrbitalPatchView>().Build();
            _garbagePatchLookUp = state.GetComponentLookup<GarbagePatch>(true);
            _localTransformLookUp = state.GetComponentLookup<LocalTransform>(true);
            state.RequireForUpdate(configuration);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            _garbagePatchLookUp.Update(ref state);
            _localTransformLookUp.Update(ref state);

            foreach ((var viewRequest, var entity) in SystemAPI.Query<RefRO<ConfigureOrbitalPatchView>>().WithEntityAccess())
            {
                // obtain orbital patch configuration from parent
                var parent = viewRequest.ValueRO.Parent;
                var patchSize = 1f;

                if (_garbagePatchLookUp.HasComponent(parent))
                {
                    var garbagePatch = _garbagePatchLookUp.GetRefRO(parent);
                    patchSize = garbagePatch.ValueRO.PatchSize;
                }

                // setup scale match patch size
                var currentTransform = _localTransformLookUp.GetRefRO(entity).ValueRO;
                entityCommandBuffer.SetComponent(entity, LocalTransform.FromPositionRotationScale(currentTransform.Position, currentTransform.Rotation, patchSize));

                // remove view request
                entityCommandBuffer.RemoveComponent<ConfigureOrbitalPatchView>(entity);
            }

            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
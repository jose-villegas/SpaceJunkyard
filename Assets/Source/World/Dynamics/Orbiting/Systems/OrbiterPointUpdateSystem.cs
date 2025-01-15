using SpaceJunkyard.World.Astronomical;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct OrbiterPointUpdateSystem : ISystem
    {
        private EntityQuery _orbitablesQuery;
        private NativeArray<AstronomicalBody> _orbitableBodies;
        
        public void OnCreate(ref SystemState state)
        {
            // state update
            state.RequireForUpdate<Orbiter>();
            var bodies = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable>().Build();
            var orbiters = SystemAPI.QueryBuilder().WithAll<Orbiter, OrbiterPoint>().Build();
            state.RequireAnyForUpdate(bodies, orbiters);
            
            _orbitablesQuery = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable, LocalTransform>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_orbitableBodies.Length == 0 || _orbitableBodies.Length != _orbitablesQuery.CalculateEntityCount())
            {
                _orbitableBodies =
                    CollectionHelper.CreateNativeArray<AstronomicalBody>(_orbitablesQuery.CalculateEntityCount(),
                        Allocator.Persistent);
            }

            // schedule orbiter point update
            var gravityCenterUpdate = new GravityCenterUpdateJob()
            {
                orbitableBodies = _orbitableBodies,
            };
            gravityCenterUpdate.ScheduleParallel();
            
            // schedule orbiter point update
            var orbiterReferenceUpdate = new OrbiterPointReferenceUpdateJob()
            {
                orbitableBodies = _orbitableBodies
            };
            orbiterReferenceUpdate.ScheduleParallel();
        }
        
        [BurstCompile]
        public partial struct GravityCenterUpdateJob : IJobEntity
        {
            public NativeArray<AstronomicalBody> orbitableBodies;

            public void Execute([EntityIndexInQuery] int index, ref AstronomicalBody astronomicalBody,
                in LocalTransform localTransform, in Orbitable _)
            {
                // update center position
                astronomicalBody.GravityCenter = localTransform.Position;
                // save copy for value updates
                orbitableBodies[index] = astronomicalBody;
            }
        }

        [BurstCompile]
        public partial struct OrbiterPointReferenceUpdateJob : IJobEntity
        {
            public NativeArray<AstronomicalBody> orbitableBodies;

            public void Execute(ref OrbiterPoint orbiterPoint)
            {
                var bodyName = orbiterPoint.Body.Name;

                // check if position from within orbitable positions
                foreach (var orbitable in orbitableBodies)
                {
                    if (orbitable.Name == bodyName)
                    {
                        orbiterPoint.ModifyCenter(orbitable.GravityCenter);
                    }
                }
            }
        }
    }
}
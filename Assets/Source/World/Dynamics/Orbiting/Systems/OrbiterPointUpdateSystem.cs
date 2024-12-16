
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

            var orbiters = SystemAPI.QueryBuilder().WithAll<Orbiter, OrbiterPoint>().Build();
            var bodies = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable>().Build();

            state.RequireAnyForUpdate(orbiters, bodies);

            _orbitablesQuery = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable, LocalTransform>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var index = 0;

            if (_orbitableBodies.Length == 0)
            {
                _orbitableBodies = CollectionHelper.CreateNativeArray<AstronomicalBody>(_orbitablesQuery.CalculateEntityCount(), Allocator.Persistent);
            }

            foreach ((var astronomicalBody, var localTransform) in SystemAPI.Query<RefRW<AstronomicalBody>, RefRO<LocalTransform>>().WithAll<Orbitable>())
            {
                // update center position
                astronomicalBody.ValueRW.GravityCenter = localTransform.ValueRO.Position;
                // save copy for value updates
                _orbitableBodies[index++] = astronomicalBody.ValueRO;
            }

            // schedule orbiter point update
            var orbiterPontUpdateJob = new OrbiterPointUpdateJob()
            {
                orbitableBodies = _orbitableBodies,
            };
            orbiterPontUpdateJob.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct OrbiterPointUpdateJob : IJobEntity
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
                        orbiterPoint.Body = orbitable;
                        break;
                    }
                }
            }
        }
    }
}
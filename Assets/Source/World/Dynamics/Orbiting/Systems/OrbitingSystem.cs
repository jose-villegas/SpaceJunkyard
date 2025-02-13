using SpaceJunkyard.World.Astronomical;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    [UpdateInGroup(typeof(TransformSystemGroup), OrderFirst = true)]
    public partial struct OrbitingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // state update
            state.RequireForUpdate<Orbiter>();

            var orbiters = SystemAPI.QueryBuilder().WithAll<Orbiter>().Build();
            var bodies = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable>().Build();

            state.RequireAnyForUpdate(orbiters, bodies);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // schedule orbiters
            var orbitingJob = new OrbitingJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime * GameDynamics.TimeMultiplier.Data,
            };
            orbitingJob.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct OrbitingJob : IJobEntity
        {
            public float deltaTime;

            public void Execute(OrbiterAspect aspect)
            {
                aspect.RotateAround(deltaTime);
            }
        }
    }
}
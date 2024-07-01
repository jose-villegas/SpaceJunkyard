using Unity.Burst;
using Unity.Entities;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public partial struct OrbitingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Orbiter>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var orbitingJob = new OrbitingJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
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
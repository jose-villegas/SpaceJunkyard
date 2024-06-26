using Unity.Entities;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public partial struct OrbitingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Orbiter>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var aspect in SystemAPI.Query<OrbiterAspect>())
            {
                aspect.RotateAround(SystemAPI.Time.DeltaTime);
            }
        }
    }
}
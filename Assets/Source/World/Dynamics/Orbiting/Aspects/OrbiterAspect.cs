using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    [BurstCompile]
    public readonly partial struct OrbiterAspect : IAspect
    {
        public readonly RefRW<Orbiter> orbiter;
        public readonly RefRO<OrbiterPoint> orbiterPoint;
        public readonly RefRW<LocalTransform> localTransform;

        [BurstCompile]
        public void RotateAround(float deltaTime)
        {
            var body = orbiterPoint.ValueRO.Body;
            var center = (float3)body.GravityCenter;

            // update position given ellipse formula    
            var position = orbiter.ValueRO.CalculateCurrentEllipticalPosition(center);
            localTransform.ValueRW.Position = position;

            // update angle on orbiter
            var mass = body.Mass;
            var radius = math.distance(center, position);
            var speed = math.sqrt(Constants.GRAV * mass / radius);

            // increase angle of orbit
            var angle = orbiter.ValueRO.CurrentAngle;
            angle = (float)(angle + speed * deltaTime);
            orbiter.ValueRW.CurrentAngle = angle;
        }
    }
}
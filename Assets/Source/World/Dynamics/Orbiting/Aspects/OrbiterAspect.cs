using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    [BurstCompile]
    public readonly partial struct OrbiterAspect : IAspect
    {
        private readonly RefRW<Orbiter> _orbiter;
        private readonly RefRO<OrbiterPoint> _orbiterPoint;
        private readonly RefRW<LocalTransform> _localTransform;

        [BurstCompile]
        public void RotateAround(float deltaTime)
        {
            var body = _orbiterPoint.ValueRO.Body;
            var center = (float3)body.GravityCenter;

            // update position given ellipse formula    
            var position = _orbiter.ValueRO.CalculateCurrentEllipticalPosition(center);
            _localTransform.ValueRW.Position = position;

            // update angle on orbiter
            var mass = body.Mass;
            var radius = math.distance(center, position);
            var speed = math.sqrt(Constants.GRAV * mass / radius);

            // increase angle of orbit
            var angle = _orbiter.ValueRO.CurrentAngle;
            angle = (float)(angle + speed * deltaTime);
            _orbiter.ValueRW.CurrentAngle = angle;
        }
    }
}
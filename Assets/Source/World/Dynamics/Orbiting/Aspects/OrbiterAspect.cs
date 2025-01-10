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

            // // update position given ellipse formula    
            // var position = _orbiter.ValueRO.CalculateCurrentEllipticalPosition(center);
            //

            //
            // var speed = math.sqrt(Constants.GRAV * mass / radius);
            // var period = (2.0 * Constants.PI * radius) / speed;
            //
            // // increase angle of orbit
            // var angle = _orbiter.ValueRO.CurrentAngle;
            // angle = (float)(angle + speed * deltaTime);
            // _orbiter.ValueRW.CurrentAngle = angle;
            
            // update angle on orbiter
            var position = _orbiter.ValueRW.UpdateAngle(center,  body.Mass, deltaTime);
            _localTransform.ValueRW.Position = position;
        }
    }
}
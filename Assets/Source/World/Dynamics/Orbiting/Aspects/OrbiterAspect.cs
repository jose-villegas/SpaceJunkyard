using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public readonly partial struct OrbiterAspect : IAspect
    {
        public readonly RefRW<Orbiter> orbiter;
        public readonly RefRO<OrbiterPoint> orbiterPoint;
        public readonly RefRW<LocalTransform> localTransform;

        [BurstCompile]
        public void RotateAround(float deltaTime)
        {
            var apogee = orbiter.ValueRO.Apogee;
            var perigee = orbiter.ValueRO.Perigee;
            var body = orbiterPoint.ValueRO.Body;
            var center = (float3)body.GravityCenter;
            var angle = orbiter.ValueRO.CurrentAngle;

            // update position given ellipse formula               
            var position = new float3(center.x + apogee * math.sin(angle), 0, center.z + perigee * math.cos(angle));
            localTransform.ValueRW.Position = position;

            // update angle on orbiter
            var mass = body.Mass;
            var radius = math.distance(center, position);
            var speed = math.sqrt(Constants.GRAV * mass / radius);

            angle = (float)(angle + speed * deltaTime);
            orbiter.ValueRW.CurrentAngle = angle;
        }
    }
}
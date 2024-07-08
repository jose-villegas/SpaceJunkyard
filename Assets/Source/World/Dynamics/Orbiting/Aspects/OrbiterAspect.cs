using Unity.Burst;
using Unity.Entities;
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
            // TODO: Calculate using approximate physics radius & mass
            var radius = orbiter.ValueRO.Radius;
            var speed = 20f;


            var angle = orbiter.ValueRO.CurrentAngle;
            angle = angle + speed * deltaTime;

            var rotation = Quaternion.Euler(0, angle, 0);
            localTransform.ValueRW.Position = rotation * new Vector3(radius, 0, 0) + orbiterPoint.ValueRO.Center;
            // update angle on orbiter
            orbiter.ValueRW.CurrentAngle = angle;
        }
    }
}
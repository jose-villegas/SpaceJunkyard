using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public readonly partial struct OrbiterAspect : IAspect
    {
        public readonly RefRW<Orbiter> orbiter;
        public readonly RefRW<LocalTransform> localTransform;

        public void RotateAround(float deltaTime) 
        {
            var speed = 20f;
            var radius = 2f;
           
            var angle = orbiter.ValueRO.CurrentAngle;
            angle = angle + speed * deltaTime;

            var rotation = Quaternion.Euler(0, angle, 0);
            localTransform.ValueRW.Position = rotation * new Vector3(radius, 0, 0);
            // update angle on orbiter
            orbiter.ValueRW.CurrentAngle = angle;
        }
    }
}
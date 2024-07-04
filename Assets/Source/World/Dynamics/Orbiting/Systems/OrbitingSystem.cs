using System.Collections.Generic;
using SpaceJunkyard.World.Astronomical;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{

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
            // extrat orbitable points
            var orbitablePoints = new List<OrbitablePoint>();

           foreach ((var localTransform, var astronomicalBody) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<AstronomicalBody>>().WithAll<Orbitable>())  
           {
                orbitablePoints.Add(new OrbitablePoint(astronomicalBody.ValueRO.Name, localTransform.ValueRO.Position));
           }

            // schedule orbiters
            var orbitingJob = new OrbitingJob()
            {
                points = new NativeArray<OrbitablePoint>(orbitablePoints.ToArray(), Allocator.TempJob),
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            orbitingJob.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct OrbitingJob : IJobEntity
        {
            public NativeArray<OrbitablePoint> points;
            public float deltaTime;

            public void Execute(OrbiterAspect aspect)
            {
                var center = Vector3.zero;

                foreach (var point in points) 
                {
                    if (point.Body == aspect.orbiter.ValueRO.Orbit.Body) center = point.Position;
                }

                aspect.RotateAround(deltaTime, center);
            }
        }
    }
}
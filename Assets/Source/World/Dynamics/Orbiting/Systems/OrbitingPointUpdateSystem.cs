
using SpaceJunkyard.World.Astronomical;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public partial struct OrbitingPointUpdateSystem : ISystem
    {
        private EntityQuery _orbitablesQuery;
        private NativeArray<UpdatePoint> _orbitablePositions;

        public void OnCreate(ref SystemState state)
        {
            // state update
            state.RequireForUpdate<Orbiter>();

            var orbiters = SystemAPI.QueryBuilder().WithAll<Orbiter, OrbiterPoint>().Build();
            var bodies = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable>().Build();

            state.RequireAnyForUpdate(orbiters, bodies);

            _orbitablesQuery = SystemAPI.QueryBuilder().WithAll<AstronomicalBody, Orbitable, LocalTransform>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var index = 0;

            if (_orbitablePositions == null || _orbitablePositions.Length == 0) 
            {
                _orbitablePositions = CollectionHelper.CreateNativeArray<UpdatePoint>(_orbitablesQuery.CalculateEntityCount(), Allocator.Persistent);
            }

            foreach ((var astronomicalBody, var localTransform) in SystemAPI.Query<RefRO<AstronomicalBody>, RefRO<LocalTransform>>().WithAll<Orbitable>())
            {
                _orbitablePositions[index++] = new UpdatePoint()
                {
                    name = astronomicalBody.ValueRO.Name,
                    position = localTransform.ValueRO.Position
                };
            }

            foreach (var orbiterPoint in SystemAPI.Query<RefRW<OrbiterPoint>>())
            {
                var bodyName = orbiterPoint.ValueRO.Body;

                // check if position from within orbitable positions
                foreach (var orbitable in _orbitablePositions)
                {
                    if (orbitable.name == bodyName)
                    {
                        orbiterPoint.ValueRW.Center = orbitable.position;
                        break;
                    }
                }
            }
        }

        private struct UpdatePoint
        {
            public Vector3 position;
            public FixedString32Bytes name;
        }
    }
}
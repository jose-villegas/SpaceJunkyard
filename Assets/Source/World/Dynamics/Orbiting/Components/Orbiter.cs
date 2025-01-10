using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    /// <summary>
    /// An entity that posses an elliptical orbit around a position
    /// </summary>
    public struct Orbiter : IComponentData
    {
        private float _radius;
        private float _currentAngle;

        public Orbiter(float radius, float angle = 0)
        {
            _currentAngle = angle;
            _radius = radius;
        }
        
        public float CurrentAngle
        {
            get => _currentAngle;
            set => _currentAngle = (float) (value % (2 * Constants.PI));
        }

        public float Radius
        {
            get => _radius;
        }

        public readonly float3 CalculateCurrentEllipticalPosition(float3 center)
        {
            return new float3(center.x + _radius * math.sin(_currentAngle), 0,
                center.z + _radius * math.cos(_currentAngle));
        }
    }
}
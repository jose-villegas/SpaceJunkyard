
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
        private float _apogee;
        private float _perigee;
        private float _currentAngle;

        public Orbiter(float apogee, float perigee) : this(apogee, perigee, 0) { }

        public Orbiter(float apogee, float perigee, float angle)
        {
            _currentAngle = angle;
            _apogee = apogee;
            _perigee = perigee;
        }


        public float CurrentAngle { get => _currentAngle; set => _currentAngle = (float)(value % (2 * Constants.PI)); }
        public float Apogee { get => _apogee; }
        public float Perigee { get => _perigee; }

        public readonly float3 CalculateCurrentEllipticalPosition(float3 center)
        {
            return new(center.x + _apogee * math.sin(_currentAngle), 0, center.z + _perigee * math.cos(_currentAngle));
        }
    }
}
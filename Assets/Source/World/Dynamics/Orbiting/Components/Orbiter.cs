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
        private float _initialAngle;
        private float _currentAngle;
        private float _currentTime;

        public Orbiter(float radius, float angle = 0)
        {
            _initialAngle = _currentAngle = angle;
            _radius = radius;
            _currentTime = 0;
        }

        public float CurrentAngle
        {
            get => _currentAngle;
            set => _currentAngle = (value % (2f * Constants.FloatPI));
        }

        public float Radius => _radius;

        public float Time => _currentTime;

        public readonly float3 CalculateCurrentEllipticalPosition(float3 center)
        {
            return new float3(center.x + _radius * math.sin(_currentAngle), 0,
                center.z + _radius * math.cos(_currentAngle));
        }

        public float3 UpdateAngle(float3 center, float mass, float deltaTime)
        {
            var speed = math.sqrt(Constants.GRAV * mass / _radius);
            // derivative of speed
            var period = (float)((2 * Constants.PI * _radius) / speed);
            // cap period of orbit using mod operator
            _currentTime = (_currentTime + deltaTime) % period;
            // linear interpolation for the angle
            CurrentAngle = _initialAngle + math.lerp(0f, 2f * Constants.FloatPI, _currentTime / period);

            return CalculateCurrentEllipticalPosition(center);
        }
    }
}
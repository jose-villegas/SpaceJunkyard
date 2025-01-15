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
        private double _radius;
        private double _initialAngle;
        private double _currentAngle;
        private double _currentTime;

        public Orbiter(double radius, double angle = 0)
        {
            _initialAngle = _currentAngle = angle;
            _radius = radius;
            _currentTime = 0;
        }

        public double CurrentAngle
        {
            get => _currentAngle;
            set => _currentAngle = (value % (2f * Constants.PI));
        }

        public double Radius => _radius;

        public double Time => _currentTime;

        public readonly float3 CalculateCurrentEllipticalPosition(float3 center)
        {
            return new float3((float)(center.x + _radius * math.sin(_currentAngle)), 0,
                (float)(center.z + _radius * math.cos(_currentAngle)));
        }

        public float3 UpdateAngle(float3 center, double mass, float deltaTime)
        {
            var period = Period(mass);
            // cap period of orbit using mod operator
            _currentTime = (_currentTime + deltaTime) % period;
            // linear interpolation for the angle
            CurrentAngle = _initialAngle + math.lerp(0.0, 2.0 * Constants.PI, _currentTime / period);

            return CalculateCurrentEllipticalPosition(center);
        }

        public readonly double Period(double mass)
        {
            var radiusAU = _radius * Constants.GAME_AU_UNIT;
            // reduction of period formula from kepler's third law using AU as distance
            return math.sqrt(math.pow(radiusAU, 3) / mass);
        }
    }
}
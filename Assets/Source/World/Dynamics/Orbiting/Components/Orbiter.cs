
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct Orbiter : IComponentData
    {
        private float _radius;
        private float _currentAngle;

        public Orbiter(float radius) : this(radius, 0) { }

        public Orbiter(float radius, float angle)
        {
            _radius = radius;
            _currentAngle = angle;
        }


        public float Radius { get => _radius; }
        public float CurrentAngle { get => _currentAngle; set => _currentAngle = value % 360f; }
    }
}

using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct Orbiter : IComponentData
    {
        private OrbitData _bodyData;
        private float _currentAngle;

        public Orbiter(OrbitData data) : this(data, 0) { }

        public Orbiter(OrbitData data, float angle)
        {
            _bodyData = data;
            _currentAngle = angle;
        }


        public OrbitData Body { get => _bodyData; }
        public float CurrentAngle { get => _currentAngle; set => _currentAngle = value % 360f; }
    }
}
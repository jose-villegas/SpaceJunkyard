
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct Orbiter : IComponentData
    {
        private Vector3 _bodyCenter;
        private float _currentAngle;

        public Orbiter(Vector3 bodyCenter)
        {
            _bodyCenter = bodyCenter;
            _currentAngle = 0f;
        }

        public Vector3 BodyCenter { get => _bodyCenter; }
        public float CurrentAngle { get => _currentAngle; set => _currentAngle = value; }
    }
}
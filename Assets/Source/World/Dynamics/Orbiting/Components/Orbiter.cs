
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct Orbiter : IComponentData
    {
        private BodyData _bodyData;
        private float _currentAngle;

        public Orbiter(BodyData data) : this(data, 0) {}

        public Orbiter(BodyData data, float angle)
        {
            _bodyData = data;
            _currentAngle = angle;
        }


        public BodyData Body { get => _bodyData; }
        public float CurrentAngle { get => _currentAngle; set => _currentAngle = value % 360f; }
    }
}
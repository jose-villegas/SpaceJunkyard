using Unity.Collections;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct OrbitData
    {
        private FixedString32Bytes _body;
        private float _radius;

        public OrbitData(FixedString32Bytes bodyName, float radius)
        {
            _body = bodyName;
            _radius = radius;
        }

        public FixedString32Bytes Body { get => _body; }
        public float Radius { get => _radius; }
    }
}
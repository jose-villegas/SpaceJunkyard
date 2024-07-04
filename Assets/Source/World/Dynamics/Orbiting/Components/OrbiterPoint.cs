using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public partial struct OrbiterPoint : IComponentData
    {
        private FixedString32Bytes _body;
        private Vector3 _center;

        public OrbiterPoint(FixedString32Bytes name, Vector3 center)
        {
            _body = name;
            _center = center;
        }

        public FixedString32Bytes Body { get => _body; }
        public Vector3 Center { get => _center; set => _center = value; }
    }
}
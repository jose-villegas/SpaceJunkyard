using Unity.Collections;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct OrbitablePoint 
        {
            private FixedString32Bytes _body;
            private Vector3 _position;

            public OrbitablePoint(FixedString32Bytes name, Vector3 position)
            {
                _body = name;
                _position = position;
            }

        public FixedString32Bytes Body { get => _body; }
        public Vector3 Position { get => _position; set => _position = value; }
    }
}
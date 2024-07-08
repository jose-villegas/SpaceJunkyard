using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public struct AstronomicalBody : IComponentData
    {
        private FixedString32Bytes _name;
        private float _mass;
        private float3 _gravityCenter;

        public AstronomicalBody(FixedString32Bytes name, float mass, Vector3 gravityCenter)
        {
            _name = name;
            _mass = mass;
            _gravityCenter = gravityCenter;
        }

        public FixedString32Bytes Name { get => _name; }
        public float Mass { get => _mass; set => _mass = value; }
        public Vector3 GravityCenter { get => _gravityCenter; set => _gravityCenter = value; }
    }
}


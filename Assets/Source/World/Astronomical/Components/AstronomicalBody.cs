using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    /// <summary>
    /// Used to identify objects with mass in space
    /// </summary>
    public struct AstronomicalBody : IComponentData
    {
        private float3 _gravityCenter;

        public AstronomicalBody(FixedString32Bytes name, float mass, Vector3 gravityCenter)
        {
            Name = name;
            Mass = mass;
            _gravityCenter = gravityCenter;
        }

        public FixedString32Bytes Name { get; }
        public float Mass { get; }

        public Vector3 GravityCenter { get => _gravityCenter; set => _gravityCenter = value; }
    }
}


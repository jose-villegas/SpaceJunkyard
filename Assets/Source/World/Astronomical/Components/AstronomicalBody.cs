using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    /// <summary>
    /// Used to identify objects with mass in space
    /// </summary>
    [Serializable]
    public struct AstronomicalBody : IComponentData
    {
        [SerializeField] private readonly FixedString32Bytes _name;
        private float3 _gravityCenter;

        public AstronomicalBody(FixedString32Bytes name, float mass, Vector3 gravityCenter)
        {
            _name = name;
            Mass = mass;
            _gravityCenter = gravityCenter;
        }

        public readonly FixedString32Bytes Name => _name;

        public float Mass { get; }

        public Vector3 GravityCenter { get => _gravityCenter; set => _gravityCenter = value; }
    }
}


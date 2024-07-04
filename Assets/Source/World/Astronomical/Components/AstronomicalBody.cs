using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public struct AstronomicalBody : IComponentData
    {
        private FixedString32Bytes _name;

        public AstronomicalBody(FixedString32Bytes name)
        {
            _name = name;
        }

        public FixedString32Bytes Name { get => _name; }
    }
}


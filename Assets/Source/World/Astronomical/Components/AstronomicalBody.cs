using Unity.Collections;
using Unity.Entities;

namespace SpaceJunkyard.World.Astronomical
{
    public struct AstronomicalBody : IComponentData
    {
        private FixedString32Bytes _name;
        private float _mass;

        public AstronomicalBody(FixedString32Bytes name, float mass)
        {
            _name = name;
            _mass = mass;
        }

        public FixedString32Bytes Name { get => _name; }
        public float Mass { get => _mass; set => _mass = value; }
    }
}


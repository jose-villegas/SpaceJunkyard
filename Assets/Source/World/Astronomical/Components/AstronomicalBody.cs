using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public struct AstronomicalBody : IComponentData 
    {
        private readonly Vector3 _position;

        public AstronomicalBody(Vector3 position)
        {
            _position = position;
        }
    }
}


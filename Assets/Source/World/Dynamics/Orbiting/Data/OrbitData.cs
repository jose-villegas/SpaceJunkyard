using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct OrbitData
    {
        private Vector3 _center;
        private float _radius;

        public OrbitData(Vector3 center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        public Vector3 Center { get => _center; }
        public float Radius { get => _radius; }
    }
}
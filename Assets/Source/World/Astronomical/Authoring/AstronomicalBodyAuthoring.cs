using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public class AstronomicalBodyAuthoring : MonoBehaviour
    {
        [SerializeField] private Vector3 _position = Vector3.zero;

        public Vector3 Position { get => _position; }

        private class Baker : Baker<AstronomicalBodyAuthoring>
        {
            public override void Bake(AstronomicalBodyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AstronomicalBody(authoring.Position));
            }
        }
    }
}
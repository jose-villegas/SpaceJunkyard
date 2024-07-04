using SpaceJunkyard.World.Astronomical;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public class OrbiterAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _body;

        public GameObject Body { get => _body; }

        private class Baker : Baker<OrbiterAuthoring>
        {
            public override void Bake(OrbiterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                // extract entity from orbiting body
                AddComponent(entity, new Orbiter(Random.Range(2.5f, 8f), Random.Range(0, 360)));
            }
        }
    }
}
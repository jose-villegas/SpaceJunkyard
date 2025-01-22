using SpaceJunkyard.World.Spacing;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public class OrbitableAuthoring : MonoBehaviour
    {
        private class Baker : Baker<OrbitableAuthoring>
        {
            public override void Bake(OrbitableAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Orbitable());
            }
        }
    }
}
using System;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public class AstronomicalBodyAuthoring : MonoBehaviour
    {
        private class Baker : Baker<AstronomicalBodyAuthoring>
        {
            public override void Bake(AstronomicalBodyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AstronomicalBody(Guid.NewGuid().ToString().Truncate(20)));
            }
        }
    }
}
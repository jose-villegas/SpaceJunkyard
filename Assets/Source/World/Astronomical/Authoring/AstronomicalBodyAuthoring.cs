using System;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public class AstronomicalBodyAuthoring : MonoBehaviour
    {
        [SerializeField] private string _name = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        [SerializeField] private float _mass = 0;

        public string Name { get => _name; }
        public float Mass { get => _mass; }

        private class Baker : Baker<AstronomicalBodyAuthoring>
        {
            public override void Bake(AstronomicalBodyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AstronomicalBody(authoring.Name, authoring.Mass));
            }
        }
    }
}
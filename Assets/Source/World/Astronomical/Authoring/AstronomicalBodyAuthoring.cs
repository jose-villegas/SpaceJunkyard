using System;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Astronomical
{
    public class AstronomicalBodyAuthoring : MonoBehaviour
    {
        [SerializeField] private string _name = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        [SerializeField] private float _mass = 0;

        public string Name => _name;
        public float Mass => _mass;

        public AstronomicalBody CreateComponent()
        {
            return new AstronomicalBody(Name, Mass, transform.position);
        }
        
        private class Baker : Baker<AstronomicalBodyAuthoring>
        {
            public override void Bake(AstronomicalBodyAuthoring authoring)
            {
                authoring.gameObject.name = authoring.Name;
                
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, authoring.CreateComponent());
            }
        }
    }
}
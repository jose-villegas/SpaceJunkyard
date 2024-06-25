using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public class SatelliteAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _body;
        
        private class Baker : Baker<SatelliteAuthoring>
        {
            public override void Bake(SatelliteAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                // extract entity from orbiting body
                var body = GetEntity(authoring._body, TransformUsageFlags.Dynamic);
                AddComponent(entity, new Satellite(body));
            }
        }
    }
}
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public class OrbiterAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _body;
        
        private class Baker : Baker<OrbiterAuthoring>
        {
            public override void Bake(OrbiterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                // extract entity from orbiting body
                AddComponent(entity, new Orbiter(authoring._body.transform.position));
            }
        }
    }
}
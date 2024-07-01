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
                var bodyData = new BodyData(authoring._body.transform.position, Random.Range(2.5f, 8f));
                AddComponent(entity, new Orbiter(bodyData, Random.Range(0, 360)));
            }
        }
    }
}
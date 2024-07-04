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
                var astronomicalBodyEntity = GetEntity(authoring.Body, TransformUsageFlags.Dynamic);
                var entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
                var bodyData = entityManager.GetComponentData<AstronomicalBody>(astronomicalBodyEntity);

                var orbit = new OrbitData(bodyData.Name, Random.Range(2.5f, 8f));
                AddComponent(entity, new Orbiter(orbit, Random.Range(0, 360)));
            }
        }
    }
}
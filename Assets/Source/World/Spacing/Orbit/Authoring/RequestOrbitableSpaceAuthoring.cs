using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    public class RequestOrbitableSpaceAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _patchesContainer;
        [SerializeField] private PatchedOrbitAreaConfigurationEntry[] _configuration;

        public PatchedOrbitAreaConfigurationEntry[] Configuration => _configuration;
        public GameObject PatchesContainer => _patchesContainer;

        public class Baker : Baker<RequestOrbitableSpaceAuthoring>
        {
            public override void Bake(RequestOrbitableSpaceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // verify we actually possess any configurations
                if (authoring.Configuration == null || authoring.Configuration.Length == 0) return;

                // pass over buffer of patch requests
                var buffer = AddBuffer<PatchedOrbitAreaConfigurationEntry>(entity);
                var containerE = GetEntity(authoring.PatchesContainer, TransformUsageFlags.Dynamic);

                for (var i = 0; i < authoring.Configuration.Length; i++)
                {
                    authoring.Configuration[i].Container = containerE;
                    buffer.Add(authoring.Configuration[i]);
                }
                
                // add buffer to contained orbital spacing entities
                AddBuffer<OrbitalAreaBufferEntry>(entity);
            }
        }
    }
}
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{

    public class RequestOrbitableSpacePatchesAuthoring : MonoBehaviour
    {
        [SerializeField] private PatchedOrbitableAreaConfiguration[] _configuration;

        public PatchedOrbitableAreaConfiguration[] Configuration { get => _configuration; }

        public class Baker : Baker<RequestOrbitableSpacePatchesAuthoring>
        {
            public override void Bake(RequestOrbitableSpacePatchesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // verify we actually posses any configurations
                if (authoring.Configuration == null || authoring.Configuration.Length == 0) return;

                // check if we have a garbage configuration
                var gConfig = authoring.Configuration.FirstOrDefault(x => x.OrbitableAreaType == OrbitableAreaType.Gargabe);
                var cConfig = authoring.Configuration.FirstOrDefault(x => x.OrbitableAreaType == OrbitableAreaType.Construction);

                AddComponent(entity, new RequestOrbitableSpacePatches(gConfig, cConfig));
            }
        }
    }
}
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    public class ConfigureOrbitalPatchViewAuthoring : MonoBehaviour 
    {
        public class Baker : Baker<ConfigureOrbitalPatchViewAuthoring>
        {
            public override void Bake(ConfigureOrbitalPatchViewAuthoring authoring)
            {
                var parentEntity = GetEntity(authoring.transform.parent.gameObject, TransformUsageFlags.Dynamic);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ConfigureOrbitalPatchView(parentEntity));
            }
        }
    }
}
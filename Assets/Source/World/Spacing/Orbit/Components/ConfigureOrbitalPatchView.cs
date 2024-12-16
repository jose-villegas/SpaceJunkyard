using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    public struct ConfigureOrbitalPatchView : IComponentData
    {
        private Entity _parent;

        public Entity Parent => _parent;

        public ConfigureOrbitalPatchView(Entity parent)
        {
            _parent = parent;
        }
    }
}
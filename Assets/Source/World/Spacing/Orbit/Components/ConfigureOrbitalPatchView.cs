using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    public struct ConfigureOrbitalPatchView : IComponentData
    {
        [SerializeField] private Entity _parent;

        public Entity Parent { get => _parent; }

        public ConfigureOrbitalPatchView(Entity parent)
        {
            _parent = parent;
        }
    }
}
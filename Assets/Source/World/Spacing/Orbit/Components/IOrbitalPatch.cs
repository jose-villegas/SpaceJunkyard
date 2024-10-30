using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public interface IOrbitalPatch : IComponentData
    {
        public Entity OrbitableBodyParent { get; }
        OrbitableAreaType OrbitableAreaType { get; }
    }
}
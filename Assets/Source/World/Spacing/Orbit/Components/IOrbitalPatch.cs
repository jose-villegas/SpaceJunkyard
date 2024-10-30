using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public interface IOrbitalPatch : IComponentData
    {
        OrbitableAreaType OrbitableAreaType { get; }
    }
}
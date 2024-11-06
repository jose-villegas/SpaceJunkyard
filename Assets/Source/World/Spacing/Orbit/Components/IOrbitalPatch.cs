using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public interface IOrbitalPatch : IComponentData
    {
        public float PatchSize { get; }
        public Entity OrbitableBody { get; }
        OrbitableAreaType OrbitableAreaType { get; }
    }
}
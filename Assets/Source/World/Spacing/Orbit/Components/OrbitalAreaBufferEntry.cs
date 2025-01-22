using SpaceJunkyard.General.Interfaces;
using SpaceJunkyard.Tools.Camera;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct OrbitalAreaBufferEntry : IEntityBufferElement
    {
        public OrbitalAreaBufferEntry(OrbitableAreaType areaType, Entity entity)
        {
            AreaType = areaType;
            Entity = entity;
        }

        public Entity Entity { get; }

        public OrbitableAreaType AreaType { get; }
    }
}
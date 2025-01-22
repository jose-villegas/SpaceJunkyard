using Unity.Entities;

namespace SpaceJunkyard.General.Interfaces
{
    public interface IEntityBufferElement : IBufferElementData
    {
        public Entity Entity { get; }
    }
}
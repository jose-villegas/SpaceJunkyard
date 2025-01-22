using Unity.Entities;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public struct GarbageInstanceEntry : IBufferElementData
    {
        public Entity Entity { get; }

        public GarbageInstanceEntry(Entity entity)
        {
            Entity = entity;
        }
    }
}
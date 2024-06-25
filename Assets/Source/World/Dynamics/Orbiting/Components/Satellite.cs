
using Unity.Entities;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public struct Satellite : IComponentData
    {
        private Entity _body;

        public Satellite(Entity body)
        {
            _body = body;
        }

        public Entity Body { get => _body; }
    }
}
using SpaceJunkyard.World.Astronomical;
using Unity.Entities;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    /// <summary>
    /// A reference to an orbitable <see cref="AstronomicalBody"/>
    /// </summary>
    public partial struct OrbiterPoint : IComponentData
    {
        private AstronomicalBody _body;

        public OrbiterPoint(AstronomicalBody body)
        {
            _body = body;
        }

        public AstronomicalBody Body { get => _body; set => _body = value; }
    }
}
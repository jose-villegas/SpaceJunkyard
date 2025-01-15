using SpaceJunkyard.World.Astronomical;
using Unity.Entities;
using Unity.Mathematics;

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

        public AstronomicalBody Body { get => _body; }

        public void ModifyCenter(float3 position)
        {
            _body.GravityCenter = position;
        }
    }
}
using SpaceJunkyard.World.Astronomical;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public class OrbiterAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _body;
        [SerializeField] private float _apogee;
        [SerializeField] private float _perigee;
        [SerializeField] private float _initialAngle;

        public GameObject Body { get => _body; }
        public float Apogee { get => _apogee; }
        public float Perigee { get => _perigee; }
        public float InitialAngle { get => _initialAngle; }

        private class Baker : Baker<OrbiterAuthoring>
        {
            public override void Bake(OrbiterAuthoring authoring)
            {
                //  we need this to be baked firstly to access component data
                DependsOn(authoring.Body);

                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                // extract entity from orbiting body & get astronomical body identifier
                var orbitable = GetComponent<AstronomicalBodyAuthoring>(authoring.Body);

                // add orbiter parameters
                AddComponent(entity, new Orbiter(authoring.Apogee, authoring.Perigee, authoring.InitialAngle));
                // add orbiting body identifier
                AddComponent(entity, new OrbiterPoint(orbitable.CreateComponent()));
            }
        }
    }
}
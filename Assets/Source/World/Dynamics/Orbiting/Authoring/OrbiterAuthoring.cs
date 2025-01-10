using SpaceJunkyard.World.Astronomical;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Dynamics.Orbiting
{
    public class OrbiterAuthoring : MonoBehaviour
    {
        /// <summary>
        /// The object we are orbiting
        /// </summary>
        [SerializeField] private GameObject _body;
        
        /// <summary>
        /// Distance between origin and orbit radius, unit is <see cref="Constants.GAME_AU_UNIT"/> for
        /// simplification using Kepler's third law.
        /// </summary>
        [SerializeField] private float _radius;
        
        /// <summary>
        /// Our initial angle in orbit determines where the body will be positioned on instanciation
        /// </summary>
        [SerializeField] private float _initialAngle;

        public GameObject Body { get => _body; }
        public float Radius { get => _radius; }
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
                AddComponent(entity, new Orbiter(authoring.Radius, authoring.InitialAngle));
                // add orbiting body identifier
                AddComponent(entity, new OrbiterPoint(orbitable.CreateComponent()));
            }
        }
    }
}

using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Tools.Camera
{
    public class CameraTracker : MonoBehaviour
    {
        private EntityManager _manager;

        private void Awake()
        {
            _manager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void Start()
        {
            var entity = _manager.CreateEntity();
            _manager.SetName(entity, name);
            _manager.AddComponentObject(entity, this);
        }
    }
}
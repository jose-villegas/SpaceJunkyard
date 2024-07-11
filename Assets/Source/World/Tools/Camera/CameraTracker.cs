
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Tools.Camera
{
    public class CameraTracker : MonoBehaviour
    {
        [SerializeField] private bool _useLerp;
        [SerializeField] private float _lerpSpeed = 1f;

        private EntityManager _manager;

        public bool UseLerp { get => _useLerp;  }
        public float LerpSpeed { get => _lerpSpeed;  }

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

using System;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Tools.Camera
{
    public class CameraTracker : MonoBehaviour
    {
        [SerializeField] private bool _useLerp;
        [SerializeField] private float _lerpSpeed = 1f;

        public bool UseLerp { get => _useLerp; }
        public float LerpSpeed { get => _lerpSpeed; }
        public Vector3 TrackedPosition { get; set; }

        private CameraTrackerSystem _trackerSystem;
        private float _currentLerpValue;

        private void Start()
        {
            _trackerSystem = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CameraTrackerSystem>();
            _trackerSystem.OnTrackedEntityChanged += OnTrackedEntityChanged;
        }

        private void OnDestroy()
        {
            _trackerSystem.OnTrackedEntityChanged -= OnTrackedEntityChanged;
        }

        private void OnTrackedEntityChanged(object sender, Entity e)
        {
            _currentLerpValue = 0f;
        }

        public void Update()
        {
            if (UseLerp && _currentLerpValue <= 1f)
            {
                _currentLerpValue += Time.deltaTime * _lerpSpeed;
                var target = _trackerSystem.TrackedTransform.Position;
                transform.position = Vector3.Lerp(transform.position, target, _currentLerpValue);
            }
            else
            {
                transform.position = _trackerSystem.TrackedTransform.Position;
            }
        }
    }
}
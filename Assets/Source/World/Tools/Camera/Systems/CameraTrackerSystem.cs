using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Tools.Camera
{
    [UpdateInGroup(typeof(TransformSystemGroup), OrderLast = true)]
    public partial class CameraTrackerSystem : SystemBase
    {
        private Entity _trackedEntity;
        public LocalToWorld TrackedTransform { get; private set; }
        public Entity TrackedEntity { get => _trackedEntity; private set => _trackedEntity = value; }

        public event EventHandler<Entity> OnTrackedEntityChanged;

        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<CameraTracked>(out var currentEntity))
            {
                return;
            }

            if (currentEntity != _trackedEntity)
            {
                _trackedEntity = currentEntity;

                OnTrackedEntityChanged.Invoke(this, _trackedEntity);
            }

            TrackedTransform = SystemAPI.GetComponent<LocalToWorld>(_trackedEntity);
        }
    }
}
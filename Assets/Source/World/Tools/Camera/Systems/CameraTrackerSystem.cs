using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SpaceJunkyard.World.Tools.Camera
{
    [UpdateInGroup(typeof(TransformSystemGroup), OrderLast = true)]
    public partial class CameraTrackerSystem : SystemBase
    {
        private Entity _trackedEntity;
        private float _lerpTimer = 0f;

        protected override void OnUpdate()
        {
            if (SystemAPI.TryGetSingletonEntity<CameraTracked>(out var currentTracked))
            {
                // tracked entity changed thus target position is different
                if (currentTracked != _trackedEntity)
                {
                    _trackedEntity = currentTracked;
                    _lerpTimer = 0;
                }

                var localToWorld = SystemAPI.GetComponent<LocalToWorld>(currentTracked);

                Entities.ForEach((CameraTracker tracker) =>
                {
                    if (tracker.UseLerp || _lerpTimer > 1f)
                    {
                        var position = tracker.transform.position;
                        _lerpTimer += SystemAPI.Time.DeltaTime * tracker.LerpSpeed;
                        tracker.transform.position = Vector3.Lerp(position, localToWorld.Position, _lerpTimer);
                    }
                    else
                    {
                        tracker.transform.position = localToWorld.Position;
                    }
                }).WithoutBurst().Run();
            }
        }
    }
}
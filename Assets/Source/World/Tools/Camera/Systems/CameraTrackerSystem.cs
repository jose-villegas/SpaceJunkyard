using Unity.Entities;
using Unity.Transforms;

namespace SpaceJunkyard.World.Tools.Camera
{
    [UpdateInGroup(typeof(TransformSystemGroup), OrderLast = true)]
    public partial class CameraTrackerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<CameraTracked>(out var trackedEntity))
                return;

            var localToWorld = SystemAPI.GetComponent<LocalToWorld>(trackedEntity);

            Entities.ForEach((CameraTracker tracker) =>
            {
                tracker.transform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
            }).WithoutBurst().Run();
        }
    }
}
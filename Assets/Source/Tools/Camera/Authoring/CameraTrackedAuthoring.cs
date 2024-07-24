using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.Tools.Camera
{
    public class CameraTrackedAuthoring : MonoBehaviour
    {
        private class Baker : Baker<CameraTrackedAuthoring>
        {
            public override void Bake(CameraTrackedAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CameraTracked());
            }
        }
    }
}
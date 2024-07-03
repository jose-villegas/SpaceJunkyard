using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Garbage.Spawning
{
    public class GarbageSpawnAssetReferenceAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _garbagePrefab;

        public GameObject GarbagePrefab { get => _garbagePrefab; }

        public class Baker : Baker<GarbageSpawnAssetReferenceAuthoring>
        {
            public override void Bake(GarbageSpawnAssetReferenceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var prefabEntity = GetEntity(authoring.GarbagePrefab, TransformUsageFlags.Dynamic);
                AddComponent(entity, new GarbageSpawnAssetReference(prefabEntity));
            }
        }
    }
}
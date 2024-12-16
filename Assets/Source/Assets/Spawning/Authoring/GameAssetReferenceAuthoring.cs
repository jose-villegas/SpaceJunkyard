using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.Assets.Spawning
{
    public class GameAssetReferenceAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _garbagePrefab;
        [SerializeField] private GameObject _orbitalPatchPrefab;

        public GameObject GarbagePrefab => _garbagePrefab;
        public GameObject OrbitalPatchPrefab => _orbitalPatchPrefab;

        public class Baker : Baker<GameAssetReferenceAuthoring>
        {
            public override void Bake(GameAssetReferenceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var garbagePrefab = GetEntity(authoring.GarbagePrefab, TransformUsageFlags.Dynamic);
                var orbitalPatchPrefab = GetEntity(authoring.OrbitalPatchPrefab, TransformUsageFlags.Dynamic);
                AddComponent(entity, new GameAssetReference(garbagePrefab, orbitalPatchPrefab));
            }
        }
    }
}
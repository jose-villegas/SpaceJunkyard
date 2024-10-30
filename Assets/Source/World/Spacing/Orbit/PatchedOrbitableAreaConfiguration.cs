using System;
using NaughtyAttributes;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    [Serializable]
    public struct PatchedOrbitableAreaConfiguration
    {
        [SerializeField] private Entity _container;
        [SerializeField] private OrbitableAreaType _orbitableAreaType;
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private int _patchCount;
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private float _centerHeight;

        public OrbitableAreaType OrbitableAreaType { get => _orbitableAreaType; }
        public float PatchCount { get => _patchCount; }
        public float CenterHeight { get => _centerHeight; }
        public Entity Container { get => _container; set => _container = value; }

        public PatchedOrbitableAreaConfiguration(Entity container, OrbitableAreaType orbitableAreaType, int patchCount, float centerHeight)
        {
            _container = container;
            _orbitableAreaType = orbitableAreaType;
            _patchCount = patchCount;
            _centerHeight = centerHeight;
        }
    }
}
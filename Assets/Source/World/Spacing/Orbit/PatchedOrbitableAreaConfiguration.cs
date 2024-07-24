using System;
using NaughtyAttributes;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    [Serializable]
    public struct PatchedOrbitableAreaConfiguration
    {
        [SerializeField] private OrbitableAreaType _orbitableAreaType;
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private float _patchSize;
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private float _baseHeight;
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private float _extension;

        public OrbitableAreaType OrbitableAreaType { get => _orbitableAreaType; }
        public float PatchSize { get => _patchSize; }
        public float BaseHeight { get => _baseHeight; }
        public float Extension { get => _extension; }

        public PatchedOrbitableAreaConfiguration(OrbitableAreaType orbitableAreaType, float patchSize, float baseHeight, float extension)
        {
            _orbitableAreaType = orbitableAreaType;
            _patchSize = patchSize;
            _baseHeight = baseHeight;
            _extension = extension;
        }
    }
}
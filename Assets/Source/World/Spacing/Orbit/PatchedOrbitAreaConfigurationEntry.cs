using System;
using NaughtyAttributes;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    /// <summary>
    /// Spacing configuration for creation of orbitable patches
    /// </summary>
    [Serializable]
    public struct PatchedOrbitAreaConfigurationEntry : IBufferElementData
    {
        Entity _container;
        [SerializeField] private OrbitableAreaType _orbitableAreaType;
        
        /// <summary>
        /// Controls the amount of patches to be instanced
        /// </summary>
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private int _patchCount;
        
        /// <summary>
        /// Controls the orbit height from the center of gravity to create this patched orbit
        /// </summary>
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private float _centerHeight;
        
        /// <summary>
        /// Controls the occupation of neighbouring patches once this patch is considered "active", useful for spacing between patches
        /// </summary>
        [SerializeField, HideIf(nameof(OrbitableAreaType), OrbitableAreaType.Disabled), AllowNesting] private int _perPatchOccupation;

        public OrbitableAreaType OrbitableAreaType { get => _orbitableAreaType; set => _orbitableAreaType = value; }
        public int PatchCount { get => _patchCount; }
        public float CenterHeight { get => _centerHeight; }
        public Entity Container { get => _container; set => _container = value; }

        public int PerPatchOccupation => _perPatchOccupation;

        public PatchedOrbitAreaConfigurationEntry(Entity container, OrbitableAreaType orbitableAreaType, int patchCount, float centerHeight, int patchOccupation)
        {
            _container = container;
            _orbitableAreaType = orbitableAreaType;
            _patchCount = patchCount;
            _centerHeight = centerHeight;
            _perPatchOccupation = patchOccupation;
        }
    }
}
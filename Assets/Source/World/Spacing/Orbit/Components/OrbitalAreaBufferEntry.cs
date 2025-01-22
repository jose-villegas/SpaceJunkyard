using System;
using SpaceJunkyard.General.Interfaces;
using Unity.Entities;
using UnityEngine;

namespace SpaceJunkyard.World.Spacing
{
    [Serializable]
    public struct OrbitalAreaBufferEntry : IEntityBufferElement
    {
        [SerializeField] private readonly Entity _entity;
        [SerializeField] private readonly OrbitableAreaType _areaType;

        public OrbitalAreaBufferEntry(OrbitableAreaType areaType, Entity entity)
        {
            _areaType = areaType;
            _entity = entity;
        }

        public readonly Entity Entity => _entity;

        public readonly OrbitableAreaType AreaType => _areaType;
    }
}
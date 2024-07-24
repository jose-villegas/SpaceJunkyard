using Unity.Collections;
using Unity.Entities;

namespace SpaceJunkyard.World.Spacing
{
    public struct RequestOrbitableSpacePatches : IComponentData
    {
        private PatchedOrbitableAreaConfiguration _garbageAreaConfiguration;
        private PatchedOrbitableAreaConfiguration _constructionAreaConfiguration;

        public PatchedOrbitableAreaConfiguration GarbageAreaConfiguration { get => _garbageAreaConfiguration; }
        public PatchedOrbitableAreaConfiguration ConstructionAreaConfiguration { get => _constructionAreaConfiguration; }

        public RequestOrbitableSpacePatches(PatchedOrbitableAreaConfiguration garbageArea, PatchedOrbitableAreaConfiguration constructionArea)
        {
            _garbageAreaConfiguration = garbageArea;
            _constructionAreaConfiguration = constructionArea;
        }
    }
}
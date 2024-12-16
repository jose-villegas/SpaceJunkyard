using Unity.Mathematics;

namespace SpaceJunkyard.Tools.Math
{
    public static class GameRandom
    {
        public static Random Utility { get; } = Random.CreateFromIndex(999999999);
    }
}
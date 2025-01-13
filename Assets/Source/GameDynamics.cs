using Unity.Burst;

namespace SpaceJunkyard
{
    public abstract class GameDynamics
    {
        public static readonly SharedStatic<int> TimeMultiplier = SharedStatic<int>.GetOrCreate<GameDynamics, IntFieldKey>();
        
        private class IntFieldKey {}
    }
}
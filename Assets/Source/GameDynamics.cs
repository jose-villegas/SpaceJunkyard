using Unity.Burst;

namespace SpaceJunkyard
{
    public abstract class GameDynamics
    {
        public static readonly SharedStatic<int> TimeMultiplier = SharedStatic<int>.GetOrCreate<GameDynamics, IntFieldKey>();
        
        // Define a Key type to identify IntField
        private class IntFieldKey {}
    }
}
using UnityEngine;

namespace SpaceJunkyard
{
    public static class GameInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitialSetup()
        {
            GameDynamics.TimeMultiplier.Data = 1;
        }
    }
}
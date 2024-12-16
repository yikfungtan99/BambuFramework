using BambuFramework.Debugging;

namespace BambuFramework
{
    public static class Bambu
    {
        public static void Log(object msgObject, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            BambuLogger.Log(msgObject.ToString(), logCategory);
        }

        public static void LogWarning(object msgObject, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            BambuLogger.LogWarning(msgObject.ToString(), logCategory);
        }

        public static void LogError(object msgObject, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            BambuLogger.LogError(msgObject.ToString(), logCategory);
        }
    }
}

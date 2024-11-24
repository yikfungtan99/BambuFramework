using BambuFramework.Debugging;

namespace BambuFramework
{
    public static class Bambu
    {
        public static void Log(object msg, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            Print(msg.ToString(), logCategory);
        }

        public static void Log(string msg, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            Print(msg, logCategory);
        }

        public static void Print(string msg, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            BambuLogger.Log(msg, logCategory);
        }
    }
}

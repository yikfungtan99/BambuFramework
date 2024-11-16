using BambuFramework.Debug;

namespace BambuFramework
{
    public static class Bambu
    {
        public static void Print(string msg, ELogCategory logCategory = ELogCategory.GENERIC)
        {
            BambuLogger.Log(msg, logCategory);
        }
    }
}

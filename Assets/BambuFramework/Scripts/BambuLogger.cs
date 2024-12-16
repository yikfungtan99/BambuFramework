using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BambuFramework.Debugging
{
    public enum ELogCategory
    {
        GENERIC = 0,
        UI = 1,
        SCENE = 2,
        INPUT = 3,
        AUDIO = 4,
        SETTING = 5
    }

    [System.Serializable]
    public class BambuLogConfig
    {
        [SerializeField] private ELogCategory error;
        public ELogCategory Error => error;

        [SerializeField] private bool showInEditor;
        [SerializeField] private bool showInBuild;
        public bool Show => GetShow();

        [SerializeField] private Color logColor = Color.white;
        public Color LogColor => logColor;

        private bool GetShow()
        {
#if UNITY_EDITOR
            return showInEditor;
#endif

#if !UNITY_EDITOR
            return showInBuild;
#endif
        }
    }

    public static class BambuLogger
    {
        // Static reference to hold the configuration (will be set via editor script)
        private static BambuLoggerConfig config;

        private static List<BambuLogConfig> logs = new List<BambuLogConfig>(); // Store registered LogConfigurations

        static BambuLogger()
        {
            InitializeConfig();
        }

        // Method to initialize config at editor time
        private static void InitializeConfig()
        {
            if (config == null)
            {
                // Find all assets of type BambuLoggerConfig in the Resources folder
                string[] guids = AssetDatabase.FindAssets("t:BambuLoggerConfig", new[] { "Assets/Resources" });

                // Check if any assets were found
                if (guids.Length > 0)
                {
                    // Get the first asset found and load it
                    string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    config = AssetDatabase.LoadAssetAtPath<BambuLoggerConfig>(assetPath);

                    if (config != null)
                    {
                        logs = config.LogConfigurations;
                        Debug.Log("BambuLoggerConfig loaded: " + config.name);
                    }
                    else
                    {
                        Debug.LogError("Failed to load BambuLoggerConfig from Resources.");
                    }
                }
                else
                {
                    Debug.LogError("No BambuLoggerConfig assets found in Resources.");
                }
            }
        }

        public static void Log(object obj, ELogCategory cat = ELogCategory.GENERIC)
        {
            LogInternal(obj, cat, LogType.Log);
        }

        public static void LogWarning(object obj, ELogCategory cat = ELogCategory.GENERIC)
        {
            LogInternal(obj, cat, LogType.Warning);
        }

        public static void LogError(object obj, ELogCategory cat = ELogCategory.GENERIC)
        {
            LogInternal(obj, cat, LogType.Error);
        }

        private static void LogInternal(object obj, ELogCategory cat, LogType logType)
        {
            BambuLogConfig target = logs.Find(x => x.Error == cat);
            if (target == null) return;
            if (!target.Show) return;

            string logMessage = $"{cat}: ";

            string message = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                                            (byte)(target.LogColor.r * 255f),
                                            (byte)(target.LogColor.g * 255f),
                                            (byte)(target.LogColor.b * 255f),
                                            logMessage) + obj;

            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Exception:
                    break;
                default:
                    break;
            }
        }
    }
}

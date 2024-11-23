using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.Debugging
{
    public enum ELogCategory
    {
        GENERIC = 0,
        UI = 1,
        SCENE = 2,
        INPUT = 3
    }

    [System.Serializable]
    public class BambuLog
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

    public class BambuLogger : SingletonBehaviour<BambuLogger>
    {
        [SerializeField] private bool showLogs = false;

        [SerializeField] private List<BambuLog> logs;

        public static void Log(object obj, ELogCategory cat = ELogCategory.GENERIC)
        {
            Instance.LogInternal(obj, cat);
        }

        public void LogInternal(object obj, ELogCategory cat)
        {
            if (!showLogs) return;

            BambuLog target = logs.Find(x => x.Error == cat);
            if (target == null) return;
            if (!target.Show) return;

            string logMessage = $"{cat}: ";

            UnityEngine.Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(target.LogColor.r * 255f), (byte)(target.LogColor.g * 255f), (byte)(target.LogColor.b * 255f), logMessage) + obj);
        }
    }
}
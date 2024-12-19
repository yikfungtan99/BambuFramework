using UnityEngine;

namespace BambuFramework.Settings
{
    public class VideoResolutionSetting : Vector2IntSetting
    {
        public override string KEY => nameof(VideoResolutionSetting);

        public override Vector2Int DefaultValue => GetDefaultResolution();

        private Vector2Int GetDefaultResolution()
        {
            var currentResolution = Screen.currentResolution; // OS-set resolution
            Vector2Int defaultResolution = new Vector2Int(currentResolution.width, currentResolution.height);
            return defaultResolution;
        }

        public override void ApplySetting()
        {
            Screen.SetResolution(Value.x, Value.y, SettingsManager.Instance.VideoWindowModeSetting.Value != 2);
        }
    }
}
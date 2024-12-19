using BambuFramework.UI;
using UnityEngine;

namespace BambuFramework.Settings
{
    public class WindowModeSetting : IntSetting
    {
        public override string KEY => nameof(WindowModeSetting);
        public override int DefaultValue => SettingsContainer.Instance.DefaultVideoWindowMode;

        public override void ApplySetting()
        {
            // Apply the actual window mode change to the application (this could be different based on your implementation)
            switch (Value)
            {
                case 0: // Fullscreen
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1: // Windowed
                    Screen.fullScreen = false;
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2: // Windowed (Borderless)
                    Screen.fullScreen = false;
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }

            Vector2Int VideoResolution = SettingsManager.Instance.ResolutionSetting.Value;
            Screen.SetResolution(VideoResolution.x, VideoResolution.y, Value != 2);
        }
    }
}
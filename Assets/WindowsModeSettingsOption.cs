using BambuFramework.Settings;
using System.Collections.Generic;

namespace BambuFramework.UI
{
    public class WindowsModeSettingsOption : CyclerSettingOption
    {
        private List<string> windowModeOptions = new List<string> { "Fullscreen", "Windowed", "Borderless" };
        private int currentIndex;

        // Override the Dropdown options property
        public override List<string> CyclerOptions
        {
            get
            {
                if (windowModeOptions == null || windowModeOptions.Count == 0)
                {
                    windowModeOptions = new List<string> { "Fullscreen", "Windowed", "Borderless" };
                }

                return windowModeOptions;
            }
        }

        private void ApplyWindowModeSetting(int currentIndex)
        {
            // Assuming SettingsManager has a method to set the window mode by index
            SettingsManager.Instance.SetVideoWindowMode(currentIndex);
        }

        public override void UpdateSettingOption()
        {
            currentIndex = SettingsManager.Instance.VideoWindowMode;  // Assuming GetWindowMode returns an index for the selected window mode

            base.UpdateSettingOption();
        }
    }
}

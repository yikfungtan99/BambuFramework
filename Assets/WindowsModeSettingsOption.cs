using BambuFramework.Settings;
using System.Collections.Generic;

namespace BambuFramework.UI
{
    public class WindowsModeSettingsOption : CyclerSettingOption
    {
        private List<string> windowModeOptions = new List<string> { "Fullscreen", "Windowed", "Borderless" };

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

        protected override void OnCycle()
        {
            // Assuming SettingsManager has a method to set the window mode by index
            SettingsManager.Instance.VideoWindowModeSetting.Set(currentIndex);
        }

        public override void UpdateSettingOption()
        {
            currentIndex = SettingsManager.Instance.VideoWindowModeSetting.Value;  // Assuming GetWindowMode returns an index for the selected window mode

            base.UpdateSettingOption();
        }
    }
}

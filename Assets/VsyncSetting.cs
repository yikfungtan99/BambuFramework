using BambuFramework.UI;
using UnityEngine;

namespace BambuFramework.Settings
{
    public class VsyncSetting : BoolSetting
    {
        public override string KEY => nameof(VsyncSetting);
        public override bool DefaultValue => SettingsContainer.Instance.DefaultVsync;

        public override void ApplySetting()
        {
            // Apply Vsync setting
            QualitySettings.vSyncCount = Value ? 1 : 0;

            // If Vsync is enabled, frame rate should sync with the display's refresh rate
            if (QualitySettings.vSyncCount == 1)
            {
                // Optional: Adjust target frame rate to be consistent with the refresh rate
                Application.targetFrameRate = -1; // Let VSync handle frame rate
            }
            else
            {
                // Set a custom frame rate cap when VSync is off
                Application.targetFrameRate = SettingsContainer.Instance.VideoFrameRates[SettingsManager.Instance.FrameRateSetting.Value];
            }

            // Log the applied Vsync setting
            Bambu.Log($"Applied Vsync: {Value}", Debugging.ELogCategory.SETTING);
        }
    }
}
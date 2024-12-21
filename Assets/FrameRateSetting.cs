using BambuFramework.UI;
using UnityEngine;

namespace BambuFramework.Settings
{
    public class FrameRateSetting : IntSetting
    {
        public override string KEY => nameof(FrameRateSetting);
        public override int DefaultValue => GetDefault();

        private int GetDefault()
        {
            double maxFrameRate = Screen.currentResolution.refreshRateRatio.value;
            var frameRates = SettingsContainer.Instance.VideoFrameRates;

            int closestIndex = 0;
            for (int i = 0; i < frameRates.Count; i++)
            {
                if (frameRates[i] <= maxFrameRate && frameRates[i] > frameRates[closestIndex])
                {
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public override void ApplySetting()
        {
            // If Vsync is enabled, frame rate should sync with the display's refresh rate
            if (QualitySettings.vSyncCount == 1)
            {
                // Optional: Adjust target frame rate to be consistent with the refresh rate
                Application.targetFrameRate = -1; // Let VSync handle frame rate
            }
            else
            {
                // Set a custom frame rate cap when VSync is off
                Application.targetFrameRate = SettingsContainer.Instance.VideoFrameRates[Value];
            }
        }
    }
}
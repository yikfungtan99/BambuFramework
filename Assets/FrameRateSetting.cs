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
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = SettingsContainer.Instance.VideoFrameRates[Value];
        }
    }
}
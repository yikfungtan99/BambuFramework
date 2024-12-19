using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class FramerateCyclerSettingOption : CyclerSettingOption
    {
        private List<string> frameRates = new List<string>();
        public override List<string> CyclerOptions
        {
            get
            {
                if (frameRates == null || frameRates.Count <= 0)
                {
                    frameRates = new List<string>();

                    foreach (var item in SettingsContainer.Instance.VideoFrameRates)
                    {
                        frameRates.Add(item.ToString());
                    }
                }

                return frameRates;
            }
        }

        protected override void OnCycle()
        {
            SettingsManager.Instance.FrameRateSetting.Set(currentIndex);
        }

        public override void UpdateSettingOption()
        {
            currentIndex = SettingsManager.Instance.FrameRateSetting.Value;
            currentIndex = Mathf.Clamp(currentIndex, 0, CyclerOptions.Count - 1);

            base.UpdateSettingOption();

            //// Initialize the label with the current option (default to 60 FPS)
            //if (label != null && CyclerOptions != null && CyclerOptions.Count > 0)
            //{
            //    label.text = CyclerOptions[currentIndex];
            //}
        }
    }
}

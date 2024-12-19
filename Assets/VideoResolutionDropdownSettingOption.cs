using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class VideoResolutionDropdownSettingOption : CyclerSettingOption
    {
        private List<string> resolutionOptions;

        public List<string> ResolutionOptions
        {
            get
            {
                if (resolutionOptions == null)
                {
                    // Populate resolution options dynamically
                    resolutionOptions = new List<string>();
                    foreach (var resolution in Screen.resolutions)
                    {
                        resolutionOptions.Add($"{resolution.width} x {resolution.height}");
                    }
                }

                return resolutionOptions;
            }
        }

        // Override the Dropdown options property
        public override List<string> CyclerOptions => ResolutionOptions;

        private void ApplyResolution(int width, int height)
        {
            // Update the resolution in the SettingsManager
            SettingsManager.Instance.SetVideoResolution(new Vector2Int(width, height));
            Bambu.Log($"Video resolution set to: {width} x {height}");
        }

        public override void UpdateSettingOption()
        {
            base.UpdateSettingOption();

            // Retrieve current resolution from the SettingsManager
            Vector2Int currentResolution = SettingsManager.Instance.VideoResolution;
            string currentResolutionString = $"{currentResolution.x} x {currentResolution.y}";


        }
    }
}

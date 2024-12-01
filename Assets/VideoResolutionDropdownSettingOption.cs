using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class VideoResolutionDropdownSettingOption : DropdownSettingOption
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

        public override string[] DropdownOptions => ResolutionOptions.ToArray();

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);
            TemplateContainer uiInstance = templateContainers[0];

            // Query the Dropdown element
            dropDown = uiInstance.Q<DropdownField>("CustomDropdown");
            if (dropDown != null)
            {
                // Retrieve current resolution from the SettingsManager
                Vector2Int currentResolution = SettingsManager.Instance.VideoResolution;
                string currentResolutionString = $"{currentResolution.x} x {currentResolution.y}";

                // Populate the dropDown and set the current value
                dropDown.choices = resolutionOptions;
                if (resolutionOptions.Contains(currentResolutionString))
                {
                    dropDown.value = currentResolutionString;
                }
                else if (resolutionOptions.Count > 0)
                {
                    dropDown.value = resolutionOptions[0]; // Fallback to the first option
                }

                // Callback for handling resolution change
                dropDown.RegisterValueChangedCallback(evt =>
                {
                    string[] resParts = evt.newValue.Split('x');
                    if (resParts.Length == 2 &&
                        int.TryParse(resParts[0].Trim(), out int width) &&
                        int.TryParse(resParts[1].Trim(), out int height))
                    {
                        ApplyResolution(width, height);
                    }
                });
            }
            else
            {
                Bambu.Log("DropdownField with the name 'settingDropdown' was not found in the UI template.", Debugging.ELogCategory.UI);
            }
        }

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

            // Populate the dropDown and set the current value
            dropDown.choices = resolutionOptions;
            if (resolutionOptions.Contains(currentResolutionString))
            {
                dropDown.SetValueWithoutNotify(currentResolutionString);
            }
            else if (resolutionOptions.Count > 0)
            {
                dropDown.SetValueWithoutNotify(resolutionOptions[0]); // Fallback to the first option
            }
        }
    }
}

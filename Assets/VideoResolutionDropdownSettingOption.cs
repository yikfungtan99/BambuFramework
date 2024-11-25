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

        public override TemplateContainer SpawnUI(SettingsMenu menu, out List<Focusable> fs)
        {
            TemplateContainer uiInstance = base.SpawnUI(menu, out fs);

            // Query the Dropdown element
            var dropdown = uiInstance.Q<DropdownField>("CustomDropdown");
            if (dropdown != null)
            {
                // Retrieve current resolution from the SettingsManager
                Vector2Int currentResolution = SettingsManager.Instance.VideoResolution;
                string currentResolutionString = $"{currentResolution.x} x {currentResolution.y}";

                // Populate the dropdown and set the current value
                dropdown.choices = resolutionOptions;
                if (resolutionOptions.Contains(currentResolutionString))
                {
                    dropdown.value = currentResolutionString;
                }
                else if (resolutionOptions.Count > 0)
                {
                    dropdown.value = resolutionOptions[0]; // Fallback to the first option
                }

                // Callback for handling resolution change
                dropdown.RegisterValueChangedCallback(evt =>
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

            return uiInstance;
        }

        private void ApplyResolution(int width, int height)
        {
            // Update the resolution in the SettingsManager
            SettingsManager.Instance.SetVideoResolution(new Vector2Int(width, height));
            Bambu.Log($"Video resolution set to: {width} x {height}");
        }
    }
}

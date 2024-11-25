using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class WindowsModeSettingsOption : DropdownSettingOption
    {
        private List<string> windowModeOptions = new List<string> { "Fullscreen", "Windowed", "Windowed (Borderless)" };
        private int currentIndex;

        // Override the Dropdown options property
        public override string[] DropdownOptions
        {
            get
            {
                if (windowModeOptions == null || windowModeOptions.Count == 0)
                {
                    windowModeOptions = new List<string> { "Fullscreen", "Windowed", "Windowed (Borderless)" };
                }

                return windowModeOptions.ToArray();
            }
        }

        public override TemplateContainer SpawnUI(SettingsMenu menu, out List<Focusable> fs)
        {
            // Create the base UI instance using the inherited method
            TemplateContainer uiInstance = base.SpawnUI(menu, out fs);

            var dropdown = uiInstance.Q<DropdownField>("CustomDropdown");

            // Set initial value for dropdown based on current window mode setting
            currentIndex = SettingsManager.Instance.VideoWindowMode;  // Assuming GetWindowMode returns an index for the selected window mode

            //if (label != null && DropdownOptions != null && DropdownOptions.Length > 0)
            //{
            //    label.text = DropdownOptions[currentIndex];  // Display the current window mode name in the label
            //}

            // Initialize the dropdown field
            if (dropdown != null)
            {
                dropdown.choices = new List<string>(DropdownOptions);
                dropdown.index = currentIndex;  // Set the dropdown to show the current index
                dropdown.RegisterValueChangedCallback(evt =>
                {
                    currentIndex = dropdown.index;
                    ApplyWindowModeSetting(currentIndex);
                    //if (label != null) label.text = DropdownOptions[currentIndex];  // Update label text
                    Bambu.Log($"Window mode changed to: {DropdownOptions[currentIndex]}", Debugging.ELogCategory.UI);
                });
            }

            // Add the dropdown and label to the focusable list
            fs.Add(dropdown);

            return uiInstance;
        }

        private void ApplyWindowModeSetting(int currentIndex)
        {
            // Assuming SettingsManager has a method to set the window mode by index
            SettingsManager.Instance.SetVideoWindowMode(currentIndex);
        }

        protected override void Focus(VisualElement template)
        {
            // Handle visual feedback when focused
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 155);
        }

        protected override void Blur(VisualElement template)
        {
            // Handle visual feedback when focus is lost
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 0);
        }
    }
}

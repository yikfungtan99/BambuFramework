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

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);
            // Create the base UI instance using the inherited method
            TemplateContainer uiInstance = templateContainers[0];

            dropDown = uiInstance.Q<DropdownField>("CustomDropdown");

            // Set initial value for dropDown based on current window mode setting
            currentIndex = SettingsManager.Instance.VideoWindowMode;  // Assuming GetWindowMode returns an index for the selected window mode

            //if (label != null && DropdownOptions != null && DropdownOptions.Length > 0)
            //{
            //    label.text = DropdownOptions[currentIndex];  // Display the current window mode name in the label
            //}

            // Initialize the dropDown field
            if (dropDown != null)
            {
                dropDown.choices = new List<string>(DropdownOptions);
                dropDown.index = currentIndex;  // Set the dropDown to show the current index

                dropDown.RegisterCallback<NavigationSubmitEvent>((click) => Expanded());
                dropDown.RegisterCallback<PointerDownEvent>((click) => Expanded());

                dropDown.RegisterCallback<FocusEvent>((click) => Collapsed());
                menu.Root.RegisterCallback<MouseDownEvent>((click) => Collapsed());

                dropDown.RegisterValueChangedCallback(evt =>
                {
                    Collapsed();
                    currentIndex = dropDown.index;
                    ApplyWindowModeSetting(currentIndex);
                    //if (label != null) label.text = DropdownOptions[currentIndex];  // Update label text
                    Bambu.Log($"Window mode changed to: {DropdownOptions[currentIndex]}", Debugging.ELogCategory.UI);
                });
            }

            // Add the dropDown and label to the focusable list
            fs.Add(dropDown);
        }

        private void Expanded()
        {
            Debug.Log("Expanded");
            SettingsManager.Instance.IsBusy = true;
        }

        private void Collapsed()
        {
            Debug.Log("Collapse");
            SettingsManager.Instance.IsBusy = false;
        }

        private void ApplyWindowModeSetting(int currentIndex)
        {
            // Assuming SettingsManager has a method to set the window mode by index
            SettingsManager.Instance.SetVideoWindowMode(currentIndex);
        }

        public override void UpdateSettingOption()
        {
            currentIndex = SettingsManager.Instance.VideoWindowMode;  // Assuming GetWindowMode returns an index for the selected window mode
            dropDown.SetValueWithoutNotify(DropdownOptions[currentIndex]);  // Set the dropDown to show the current index
        }
    }
}

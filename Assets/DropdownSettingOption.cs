using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class DropdownSettingOption : SettingOption
    {
        [SerializeField] private string[] baseOptions;
        public string[] DropdownOptions { get => baseOptions; }
        public override ESettingOptions SettingsOption => ESettingOptions.DROPDOWN;

        public override TemplateContainer SpawnUI()
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI();

            // Query the Dropdown element
            var dropdown = uiInstance.Q<DropdownField>("CustomDropdown");
            if (dropdown != null)
            {
                // Populate the dropdown with the options
                dropdown.choices = new List<string>(DropdownOptions);

                // Optionally set the default value
                if (DropdownOptions.Length > 0)
                {
                    dropdown.value = DropdownOptions[0]; // Set the first option as the default
                }

                // Optionally, add a callback for when the dropdown value changes
                dropdown.RegisterValueChangedCallback(evt =>
                {
                    Bambu.Log($"Dropdown value changed to: {evt.newValue}");
                });
            }
            else
            {
                Bambu.Log("DropdownField with the name 'settingDropdown' was not found in the UI template.");
            }

            return uiInstance;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class DropdownSettingOption : SettingOption
    {
        [SerializeField] private string[] baseOptions;
        public virtual string[] DropdownOptions { get => baseOptions; }
        public override ESettingOptions SettingsOption => ESettingOptions.DROPDOWN;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);

            // Clone the base template
            TemplateContainer uiInstance = templateContainers[0];

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
                    Bambu.Log($"Dropdown value changed to: {evt.newValue}", Debugging.ELogCategory.UI);
                });
            }
            else
            {
                Bambu.Log("DropdownField with the name 'settingDropdown' was not found in the UI template.", Debugging.ELogCategory.UI);
            }

            fs.Add(dropdown);

            dropdown.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            dropdown.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        }

        protected override void Focus(VisualElement template)
        {
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 155);
        }

        protected override void Blur(VisualElement template)
        {
            Color initColor = template.style.backgroundColor.value;
            template.style.backgroundColor = new Color(initColor.r, initColor.g, initColor.b, 0);
        }
    }
}

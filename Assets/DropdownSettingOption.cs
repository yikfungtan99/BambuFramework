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

        public override TemplateContainer SpawnUI(SettingsMenu menu, out List<Focusable> fs)
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI(menu, out fs);

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

            return uiInstance;
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

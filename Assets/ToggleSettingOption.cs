using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class ToggleSettingOption : SettingOption
    {
        public override ESettingOptions SettingsOption => ESettingOptions.TOGGLE;

        public override TemplateContainer SpawnUI(SettingsMenu menu, out List<Focusable> fs)
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI(menu, out fs);

            // Query the slider and text field elements
            var toggle = uiInstance.Q<Toggle>("CustomToggle");
            fs.Add(toggle);

            toggle.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            toggle.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));

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

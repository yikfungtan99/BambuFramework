﻿using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class ToggleSettingOption : SettingOption
    {
        public override ESettingOptions SettingsOption => ESettingOptions.TOGGLE;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);

            // Clone the base template
            TemplateContainer uiInstance = templateContainers[0];

            // Query the slider and text field elements
            var toggle = uiInstance.Q<Toggle>("CustomToggle");
            fs.Add(toggle);

            toggle.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
            toggle.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
        }

        public override void UpdateSettingOption()
        {

        }
    }
}

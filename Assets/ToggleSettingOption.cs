using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class ToggleSettingOption : SettingOption
    {
        public override ESettingOptions SettingsOption => ESettingOptions.TOGGLE;

        public override TemplateContainer SpawnUI(out List<Focusable> fs)
        {
            // Clone the base template
            TemplateContainer uiInstance = base.SpawnUI(out fs);

            // Query the slider and text field elements
            var toggle = uiInstance.Q<Toggle>("CustomToggle");
            fs.Add(toggle);

            return uiInstance;
        }
    }
}

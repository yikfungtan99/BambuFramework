using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    public class VsyncToggleOption : ToggleSettingOption
    {
        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);

            if (toggle != null)
            {
                UpdateSettingOption();

                // Register a callback to update the GameplayConsole setting when the toggle changes
                toggle.RegisterValueChangedCallback(evt =>
                {
                    // Update the GameplayConsole setting in SettingsManager
                    SettingsManager.Instance.VsyncSetting.Set(evt.newValue);
                });
            }
            else
            {
                Bambu.Log("Toggle with the name 'CustomToggle' was not found in the UI template.");
            }
        }

        public override void UpdateSettingOption()
        {
            toggle.value = SettingsManager.Instance.VsyncSetting.Value;
        }
    }
}

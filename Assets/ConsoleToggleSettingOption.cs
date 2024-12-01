using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class ConsoleToggleSettingOption : ToggleSettingOption
    {
        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> fs)
        {
            base.SpawnUI(menu, out templateContainers, out fs);
            // Create the base UI instance using the inherited method
            TemplateContainer uiInstance = templateContainers[0];

            // Query the Toggle element in the template
            var toggle = uiInstance.Q<Toggle>("CustomToggle");
            if (toggle != null)
            {
                // Set the initial value based on the current GameplayConsole setting
                toggle.value = SettingsManager.Instance.GameplayConsole;

                // Register a callback to update the GameplayConsole setting when the toggle changes
                toggle.RegisterValueChangedCallback(evt =>
                {
                    // Update the GameplayConsole setting in SettingsManager
                    SettingsManager.Instance.SetGameplayConsole(evt.newValue);
                    Bambu.Log($"Gameplay Console set to: {evt.newValue}", Debugging.ELogCategory.UI);
                });
            }
            else
            {
                Bambu.Log("Toggle with the name 'CustomToggle' was not found in the UI template.");
            }
        }
    }
}

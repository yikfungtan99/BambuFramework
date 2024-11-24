using System.Collections.Generic;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class SettingsTab
    {
        public string TabName; // Name of the tab (e.g., "AudioReference", "Graphics")
        public List<SettingOption> SettingOptions; // List of SettingOptions for this tab
    }
}

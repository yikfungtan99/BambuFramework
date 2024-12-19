using BambuFramework.UI;
using IngameDebugConsole;

namespace BambuFramework.Settings
{
    public class ConsoleSetting : BoolSetting
    {
        public override string KEY => nameof(ConsoleSetting);
        public override bool DefaultValue => SettingsContainer.Instance.DefaultGameplayConsole;

        public override void ApplySetting()
        {
            DebugLogManager.Instance.gameObject.SetActive(Value);
            Bambu.Log($"Applied Console: {Value}", Debugging.ELogCategory.SETTING);
        }
    }
}
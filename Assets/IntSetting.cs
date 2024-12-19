using UnityEngine;

namespace BambuFramework.Settings
{
    public abstract class IntSetting : Setting<int>
    {
        public override void SaveSetting()
        {
            Bambu.Log($"Save {KEY}: {Value}", Debugging.ELogCategory.SETTING);
            PlayerPrefs.SetInt(KEY, Value);
        }

        public override void LoadSetting()
        {
            int loadValue = PlayerPrefs.GetInt(KEY, DefaultValue);
            Bambu.Log($"Load {KEY}: {loadValue}", Debugging.ELogCategory.SETTING);
            base.Set(loadValue);
        }

        public override void RevertDefaultSetting()
        {
            Set(DefaultValue);
        }

        public override void RevertSetting()
        {
            LoadSetting();
        }

        public override bool HaveChanges()
        {
            return Value != PlayerPrefs.GetInt(KEY);
        }
        public override bool HaveChangesFromDefault()
        {
            return Value != DefaultValue;
        }
    }
}

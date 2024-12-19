using UnityEngine;

namespace BambuFramework.Settings
{
    public abstract class BoolSetting : Setting<bool>
    {
        public override void SaveSetting()
        {
            PlayerPrefs.SetInt(KEY, Value ? 1 : 0);
        }

        public override void LoadSetting()
        {
            Set(PlayerPrefs.GetInt(KEY, DefaultValue ? 1 : 0) == 1);
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
            int value = Value == false ? 0 : 1;
            return value != PlayerPrefs.GetInt(KEY);
        }
        public override bool HaveChangesFromDefault()
        {
            return Value != DefaultValue;
        }
    }
}

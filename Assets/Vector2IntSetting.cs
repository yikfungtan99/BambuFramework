using UnityEngine;

namespace BambuFramework.Settings
{
    public abstract class Vector2IntSetting : Setting<Vector2Int>
    {
        public override void SaveSetting()
        {
            PlayerPrefs.SetInt(KEY + "_width", Value.x);
            PlayerPrefs.SetInt(KEY + "_height", Value.y);
        }

        public override void LoadSetting()
        {
            Set(new Vector2Int(PlayerPrefs.GetInt(KEY + "_width", DefaultValue.x), PlayerPrefs.GetInt(KEY + "_height", DefaultValue.y)));
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
            return Value.x != PlayerPrefs.GetInt(KEY + "_width") || Value.y != PlayerPrefs.GetInt(KEY + "_height");
        }
        public override bool HaveChangesFromDefault()
        {
            return Value != DefaultValue;
        }
    }
}

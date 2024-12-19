namespace BambuFramework.Settings
{
    public abstract class SettingBase
    {
        public abstract string KEY { get; }
        public abstract void SaveSetting();
        public abstract void LoadSetting();
        public abstract void RevertSetting();
        public abstract void RevertDefaultSetting();
        public abstract bool HaveChanges();
        public abstract bool HaveChangesFromDefault();
        public abstract void ApplySetting();
    }

    public abstract class Setting<T> : SettingBase
    {
        private T value;
        public abstract T DefaultValue { get; }
        public virtual T Value { get => value; }

        public virtual void Set(T value)
        {
            this.value = value;
            Bambu.Log($"{this} set to: {value}", Debugging.ELogCategory.SETTING);
        }
    }
}

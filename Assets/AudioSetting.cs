using BambuFramework.Audio;
using BambuFramework.UI;

namespace BambuFramework.Settings
{
    public abstract class AudioSetting : IntSetting
    {
        public abstract EAudioChannel audioChannel { get; }
        public override string KEY => audioChannel + "_SETTING";
        public override int DefaultValue => SettingsContainer.Instance.DefaultAudioMaster;

        public override void Set(int value)
        {
            base.Set(value);
            AudioManager.Instance.SetChannelVolume(audioChannel, Value);

        }

        public override void ApplySetting()
        {
            AudioManager.Instance.SetChannelVolume(audioChannel, Value);
        }
    }
}
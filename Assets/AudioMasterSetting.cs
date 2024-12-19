using BambuFramework.Audio;

namespace BambuFramework.Settings
{

    public class AudioMasterSetting : AudioSetting
    {
        public override EAudioChannel audioChannel => EAudioChannel.MASTER;
    }
}
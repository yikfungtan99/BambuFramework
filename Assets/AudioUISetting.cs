using BambuFramework.Audio;

namespace BambuFramework.Settings
{
    public class AudioUISetting : AudioSetting
    {
        public override EAudioChannel audioChannel => EAudioChannel.UI;
    }
}
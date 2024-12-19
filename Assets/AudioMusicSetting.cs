using BambuFramework.Audio;

namespace BambuFramework.Settings
{
    public class AudioMusicSetting : AudioSetting
    {
        public override EAudioChannel audioChannel => EAudioChannel.MUSIC;
    }
}
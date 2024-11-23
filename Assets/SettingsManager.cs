using BambuFramework.UI;
using UnityEngine;

namespace BambuFramework.Settings
{
    public class SettingsManager : SingletonBehaviour<SettingsManager>
    {
        private SettingsContainer settingsContainer;

        private readonly string KEY_SETTING_GAMEPLAY_CONSOLE = nameof(KEY_SETTING_GAMEPLAY_CONSOLE);

        private readonly string KEY_SETTING_VIDEO_RESOLUTION = nameof(KEY_SETTING_VIDEO_RESOLUTION);
        private readonly string KEY_SETTING_VIDEO_WINDOW_MODE = nameof(KEY_SETTING_VIDEO_WINDOW_MODE);
        private readonly string KEY_SETTING_VIDEO_FRAMERATE = nameof(KEY_SETTING_VIDEO_FRAMERATE);

        private readonly string KEY_SETTING_AUDIO_MASTER = nameof(KEY_SETTING_AUDIO_MASTER);
        private readonly string KEY_SETTING_AUDIO_SFX = nameof(KEY_SETTING_AUDIO_SFX);
        private readonly string KEY_SETTING_AUDIO_MUSIC = nameof(KEY_SETTING_AUDIO_MUSIC);

        public bool GameplayConsole { get; private set; }
        public Vector2Int VideoResolution { get; private set; }
        public int VideoWindowMode { get; private set; }
        public int VideoFramerate { get; private set; }
        public float AudioMaster { get; private set; }
        public float AudioSFX { get; private set; }
        public float AudioMusic { get; private set; }

        private void Start()
        {
            settingsContainer = SettingsContainer.Instance;
            LoadSettings();
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetInt(KEY_SETTING_GAMEPLAY_CONSOLE, GameplayConsole ? 1 : 0);

            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Width", VideoResolution.x);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Height", VideoResolution.y);

            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_WINDOW_MODE, VideoWindowMode);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_FRAMERATE, VideoFramerate);

            PlayerPrefs.SetFloat(KEY_SETTING_AUDIO_MASTER, AudioMaster);
            PlayerPrefs.SetFloat(KEY_SETTING_AUDIO_SFX, AudioSFX);
            PlayerPrefs.SetFloat(KEY_SETTING_AUDIO_MUSIC, AudioMusic);

            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            GameplayConsole = PlayerPrefs.GetInt(KEY_SETTING_GAMEPLAY_CONSOLE, 0) == 1;

            LoadVideoResolution();
            VideoWindowMode = PlayerPrefs.GetInt(KEY_SETTING_VIDEO_WINDOW_MODE, 1);
            VideoFramerate = PlayerPrefs.GetInt(KEY_SETTING_VIDEO_FRAMERATE, 0);

            AudioMaster = PlayerPrefs.GetFloat(KEY_SETTING_AUDIO_MASTER, 50f);
            AudioSFX = PlayerPrefs.GetFloat(KEY_SETTING_AUDIO_SFX, 50f);
            AudioMusic = PlayerPrefs.GetFloat(KEY_SETTING_AUDIO_MUSIC, 50f);
        }

        public void LoadVideoResolution()
        {
            int width = PlayerPrefs.GetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Width", Screen.currentResolution.width);
            int height = PlayerPrefs.GetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Height", Screen.currentResolution.height);
            VideoResolution = new Vector2Int(width, height);
        }

        public void SetVideoResolution(Vector2Int resolution)
        {
            VideoResolution = resolution;
            SaveSettings();
        }

        public void SetGameplayConsole(bool value)
        {
            GameplayConsole = value;
            SaveSettings();
        }

        public void SetVideoWindowMode(int value)
        {
            VideoWindowMode = value;

            // Apply the actual window mode change to the application (this could be different based on your implementation)
            switch (VideoWindowMode)
            {
                case 0: // Fullscreen
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1: // Windowed
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2: // Windowed (Borderless)
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }

            SaveSettings();
        }

        public void SetVideoFramerate(int value)
        {
            VideoFramerate = value;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = settingsContainer.VideoFrameRates[VideoFramerate];

            SaveSettings();
        }

        public void SetAudioMaster(float value)
        {
            AudioMaster = value;
            SaveSettings();
        }

        public void SetAudioSFX(float value)
        {
            AudioSFX = value;
            SaveSettings();
        }

        public void SetAudioMusic(float value)
        {
            AudioMusic = value;
            SaveSettings();
        }

        public void ResetVideoSettings()
        {
            VideoResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height); // Default to monitor resolution
            VideoWindowMode = 1; // Default to fullscreen
            VideoFramerate = 60; // Default to 60 FPS

            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Width", VideoResolution.x);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Height", VideoResolution.y);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_WINDOW_MODE, VideoWindowMode);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_FRAMERATE, VideoFramerate);

            PlayerPrefs.Save();
        }

        public void SetAudioVolume(EAudioChannel channel, float volume)
        {
            switch (channel)
            {
                case EAudioChannel.MASTER:
                    SetAudioMaster(volume);
                    break;
                case EAudioChannel.SFX:
                    SetAudioSFX(volume);
                    break;
                case EAudioChannel.MUSIC:
                    SetAudioMusic(volume);
                    break;
                default:
                    break;
            }
        }

        public float GetAudioVolume(EAudioChannel channel)
        {
            switch (channel)
            {
                case EAudioChannel.MASTER:
                    return AudioMaster;
                case EAudioChannel.SFX:
                    return AudioSFX;
                case EAudioChannel.MUSIC:
                    return AudioMusic;
                default:
                    return 0;
            }
        }
    }
}

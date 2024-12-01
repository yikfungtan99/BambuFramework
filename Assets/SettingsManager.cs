using BambuFramework.Audio;
using BambuFramework.UI;
using IngameDebugConsole;
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
        public bool IsFullScreen
        {
            get
            {
                return VideoWindowMode == 0;
            }
        }
        public int VideoWindowMode { get; private set; }
        public int VideoFramerate { get; private set; }

        public int AudioMaster { get; private set; }
        public int AudioSFX { get; private set; }
        public int AudioMusic { get; private set; }

        private void Start()
        {
            settingsContainer = SettingsContainer.Instance;

            LoadSettings();
        }

        public void SaveSettings()
        {
            SaveGameplaySettings();

            SaveVideoSettings();

            SaveAudioSettings();

            PlayerPrefs.Save();
        }

        private void SaveGameplaySettings()
        {
            PlayerPrefs.SetInt(KEY_SETTING_GAMEPLAY_CONSOLE, GameplayConsole ? 1 : 0);
        }

        private void SaveVideoSettings()
        {
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Width", VideoResolution.x);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_RESOLUTION + "_Height", VideoResolution.y);

            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_WINDOW_MODE, VideoWindowMode);
            PlayerPrefs.SetInt(KEY_SETTING_VIDEO_FRAMERATE, VideoFramerate);
        }

        private void SaveAudioSettings()
        {
            PlayerPrefs.SetInt(KEY_SETTING_AUDIO_MASTER, AudioMaster);
            PlayerPrefs.SetInt(KEY_SETTING_AUDIO_SFX, AudioSFX);
            PlayerPrefs.SetInt(KEY_SETTING_AUDIO_MUSIC, AudioMusic);
        }

        public void LoadSettings()
        {
            GameplayConsole = PlayerPrefs.GetInt(KEY_SETTING_GAMEPLAY_CONSOLE, 0) == 1;
            SetGameplayConsole(GameplayConsole);

            VideoWindowMode = PlayerPrefs.GetInt(KEY_SETTING_VIDEO_WINDOW_MODE, 1);
            VideoFramerate = PlayerPrefs.GetInt(KEY_SETTING_VIDEO_FRAMERATE, 0);
            LoadVideoResolution();

            AudioMaster = PlayerPrefs.GetInt(KEY_SETTING_AUDIO_MASTER, 50);
            AudioSFX = PlayerPrefs.GetInt(KEY_SETTING_AUDIO_SFX, 50);
            AudioMusic = PlayerPrefs.GetInt(KEY_SETTING_AUDIO_MUSIC, 50);

            SetAudioMaster(AudioMaster);
            SetAudioSFX(AudioSFX);
            SetAudioMusic(AudioMusic);
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
            Screen.SetResolution(VideoResolution.x, VideoResolution.y, IsFullScreen);
            SaveVideoSettings();
        }

        public void SetGameplayConsole(bool value)
        {
            GameplayConsole = value;

            DebugLogManager.Instance.gameObject.SetActive(GameplayConsole);

            SaveGameplaySettings();
        }

        public void SetVideoWindowMode(int value)
        {
            VideoWindowMode = value;
            Bambu.Log(VideoWindowMode);

            // Apply the actual window mode change to the application (this could be different based on your implementation)
            switch (VideoWindowMode)
            {
                case 0: // Fullscreen
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1: // Windowed
                    Screen.fullScreen = false;
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2: // Windowed (Borderless)
                    Screen.fullScreen = false;
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }

            Screen.SetResolution(VideoResolution.x, VideoResolution.y, IsFullScreen);
            SaveVideoSettings();
        }

        public void SetVideoFramerate(int value)
        {
            VideoFramerate = value;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = settingsContainer.VideoFrameRates[VideoFramerate];

            SaveVideoSettings();
        }

        public void SetAudioMaster(int value)
        {
            AudioMaster = value;
            AudioManager.Instance.SetChannelVolume(EAudioChannel.MASTER, AudioMaster);
            SaveAudioSettings();
        }

        public void SetAudioSFX(int value)
        {
            AudioSFX = value;
            AudioManager.Instance.SetChannelVolume(EAudioChannel.SFX, AudioSFX);
            SaveAudioSettings();
        }

        public void SetAudioMusic(int value)
        {
            AudioMusic = value;
            AudioManager.Instance.SetChannelVolume(EAudioChannel.MUSIC, AudioMusic);
            SaveAudioSettings();
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

        public void SetAudioVolume(EAudioChannel channel, int volume)
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

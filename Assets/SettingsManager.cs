using BambuFramework.Audio;
using BambuFramework.UI;
using IngameDebugConsole;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

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

        private readonly string KEY_INPUT_BINDS = nameof(KEY_INPUT_BINDS);

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
        public bool IsRebinding { get; private set; }

        string rebindingControlScheme;
        Action OnRebindComplete;
        InputAction inputAction;

        InputActionRebindingExtensions.RebindingOperation rebindingOperation;

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

            LoadRebinds();
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

        public void RevertDefaultGameplaySettings()
        {
            GameplayConsole = SettingsContainer.Instance.DefaultGameplayConsole;
            SetGameplayConsole(GameplayConsole);
        }

        public void RevertDefaultVideoSettings()
        {
            var currentResolution = Screen.currentResolution; // OS-set resolution
            Vector2Int defaultResolution = new Vector2Int(currentResolution.width, currentResolution.height);

            VideoResolution = defaultResolution;

            VideoWindowMode = SettingsContainer.Instance.DefaultVideoWindowMode;

            VideoFramerate = SettingsContainer.Instance.VideoFrameRates.Count - 1;

            SetVideoResolution(VideoResolution);
            SetVideoWindowMode(VideoWindowMode);
            SetVideoFramerate(VideoFramerate);
        }

        public void RevertDefaultAudioSettings()
        {
            AudioMaster = SettingsContainer.Instance.DefaultAudioMaster;
            AudioSFX = SettingsContainer.Instance.DefaultAudioSFX;
            AudioMusic = SettingsContainer.Instance.DefaultAudioMusic;

            SetAudioMaster(AudioMaster);
            SetAudioSFX(AudioSFX);
            SetAudioMusic(AudioMusic);
        }

        public void RevertDefaultInputSettings()
        {
            var playerInput = PlayerManager.Instance.HostPlayer.PlayerInput;
            playerInput.actions.RemoveAllBindingOverrides();

            SaveRebinds();
        }

        public void RebindKeys(Player player, InputAction ia, Action onComplete = null)
        {
            IsRebinding = true;

            inputAction = ia;
            inputAction.Disable();

            player.OnInputDeviceChanged += CancelRebind;

            // Get the active control scheme from the first player (or adjust based on your system)
            rebindingControlScheme = GetActiveControlScheme();

            OnRebindComplete = onComplete;

            if (rebindingControlScheme == "Gamepad")
            {
                // Filter the bindings to only include the ones for the active control scheme
                rebindingOperation = inputAction
                                    .PerformInteractiveRebinding()
                                    .WithBindingGroup(rebindingControlScheme)
                                    .WithControlsExcluding("Keyboard")
                                    .WithControlsExcluding("Mouse")
                                    .WithCancelingThrough("<Gamepad>/select")
                                    .OnMatchWaitForAnother(0.1f)  // Optional, to wait for another match
                                    .OnComplete(op =>
                                    {
                                        Bambu.Log("COMPLETED Gamepad", Debugging.ELogCategory.SETTING);
                                        // Update the button text to the new binding
                                        onComplete?.Invoke();
                                        inputAction.Enable();
                                        IsRebinding = false;

                                        op.Dispose();

                                        SaveRebinds();
                                        rebindingOperation = null;
                                    })
                                    .OnCancel(op =>
                                    {
                                        Bambu.Log("CANCELLED Gamepad", Debugging.ELogCategory.SETTING);
                                        // Update the button text to the new binding
                                        onComplete?.Invoke();
                                        inputAction.Enable();
                                        IsRebinding = false;

                                        op.Dispose();

                                        rebindingOperation = null;
                                    })
                                    .Start();
            }

            if (rebindingControlScheme == "Keyboard&Mouse")
            {
                // Filter the bindings to only include the ones for the active control scheme
                rebindingOperation = inputAction.PerformInteractiveRebinding()
                    .WithBindingGroup(rebindingControlScheme)
                    .WithControlsExcluding("Gamepad")
                    .WithCancelingThrough("<Keyboard>/escape")
                    .OnMatchWaitForAnother(0.1f)  // Optional, to wait for another match
                    .OnComplete(op =>
                    {
                        Bambu.Log("COMPLETED keyboard", Debugging.ELogCategory.SETTING);

                        // Update the button text to the new binding
                        onComplete?.Invoke();
                        inputAction.Enable();
                        IsRebinding = false;

                        op.Dispose();

                        SaveRebinds();
                        rebindingOperation = null;
                    })
                    .OnCancel(op =>
                    {
                        Bambu.Log("Cancelled keyboard", Debugging.ELogCategory.SETTING);
                        onComplete?.Invoke();
                        inputAction.Enable();
                        IsRebinding = false;

                        op.Dispose();

                        rebindingOperation = null;
                    })
                    .Start();
            }
        }

        public void CancelRebind()
        {
            Bambu.Log("CANCEL REBIND", Debugging.ELogCategory.SETTING);
            if (rebindingOperation != null) rebindingOperation.Cancel();
        }

        private void SaveRebinds()
        {
            var rebinds = PlayerManager.Instance.HostPlayer.PlayerInput.actions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(KEY_INPUT_BINDS, rebinds);
        }

        private void LoadRebinds()
        {
            var rebinds = PlayerPrefs.GetString(KEY_INPUT_BINDS);
            PlayerManager.Instance.HostPlayer.PlayerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }

        private string GetActiveControlScheme()
        {
            var user = InputUser.all[0];  // Assumes single-player or first player
            return user.controlScheme?.name ?? "Keyboard&Mouse";  // Return the current control scheme or fallback to default
        }

        public void RevertDefaultSettings(int index)
        {
            switch (index)
            {
                case 0:
                    RevertDefaultGameplaySettings();
                    break;

                case 1:
                    RevertDefaultVideoSettings();
                    break;

                case 2:
                    RevertDefaultAudioSettings();
                    break;

                case 3:
                    RevertDefaultInputSettings();
                    break;

                default:
                    break;
            }
        }
    }
}

using BambuFramework.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace BambuFramework.Settings
{
    public class SettingsManager : SingletonBehaviour<SettingsManager>
    {
        private List<SettingBase> settings = new List<SettingBase>();
        private List<SettingBase> gameplaySettings = new List<SettingBase>();
        private List<SettingBase> videoSettings = new List<SettingBase>();
        private List<SettingBase> audioSettings = new List<SettingBase>();

        //GAMEPLAY
        public ConsoleSetting ConsoleSetting = new ConsoleSetting();

        //VIDEO
        public VideoResolutionSetting ResolutionSetting = new VideoResolutionSetting();
        public VsyncSetting VsyncSetting = new VsyncSetting();
        public WindowModeSetting VideoWindowModeSetting = new WindowModeSetting();
        public FrameRateSetting FrameRateSetting = new FrameRateSetting();

        public AudioMasterSetting AudioMasterSettings = new AudioMasterSetting();
        public AudioSFXSetting AudioSFXSettings = new AudioSFXSetting();
        public AudioMusicSetting AudioMusicSettings = new AudioMusicSetting();
        public AudioUISetting AudioUISettings = new AudioUISetting();

        private readonly string KEY_INPUT_BINDS = nameof(KEY_INPUT_BINDS);

        public bool IsBusy { get; set; }

        string rebindingControlScheme;
        Action OnRebindComplete;
        InputAction inputAction;

        InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        public List<SettingBase> GetSettings(int index)
        {
            switch (index)
            {
                case 0:
                    return gameplaySettings;

                case 1:
                    return videoSettings;

                case 2:
                    return audioSettings;
            }

            return null;
        }

        private void Start()
        {
            gameplaySettings.Add(ConsoleSetting);

            videoSettings.Add(ResolutionSetting);
            videoSettings.Add(VideoWindowModeSetting);
            videoSettings.Add(FrameRateSetting);

            audioSettings.Add(AudioMasterSettings);
            audioSettings.Add(AudioSFXSettings);
            audioSettings.Add(AudioMusicSettings);
            audioSettings.Add(AudioUISettings);

            settings.AddRange(gameplaySettings);
            settings.AddRange(videoSettings);
            settings.AddRange(audioSettings);

            LoadSettings();
            ApplySettings();
        }

        public void ApplySettings()
        {
            foreach (var setting in settings)
            {
                setting.ApplySetting();
            }
        }

        public void ApplySetting(int index)
        {
            foreach (var setting in GetSettings(index))
            {
                setting.ApplySetting();
                setting.SaveSetting();
            }
        }

        public void LoadSetting(int index)
        {
            foreach (var setting in GetSettings(index))
            {
                setting.LoadSetting();
            }
        }

        public void LoadSettings()
        {
            foreach (var setting in settings)
            {
                setting.LoadSetting();
            }

            LoadRebinds();
        }

        public void RevertDefaultSettings(int index)
        {
            Debug.Log(index);
            if (index == 3)
            {
                RevertDefaultInputSettings();
                return;
            }

            foreach (var setting in GetSettings(index))
            {
                setting.RevertDefaultSetting();
                setting.ApplySetting();
                setting.SaveSetting();
            }
        }

        public void RevertDefaultInputSettings()
        {
            var playerInput = PlayerManager.Instance.HostPlayer.PlayerInput;
            playerInput.actions.RemoveAllBindingOverrides();

            SaveRebinds();
        }

        public bool HaveChanges()
        {
            foreach (SettingBase setting in settings)
            {
                if (setting.HaveChanges())
                {
                    return true;
                }
            }

            return false;
        }

        public void RebindKeys(Player player, InputAction ia, Action onComplete = null)
        {
            IsBusy = true;

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
                                        IsBusy = false;

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
                                        IsBusy = false;

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
                        IsBusy = false;

                        op.Dispose();

                        SaveRebinds();
                        rebindingOperation = null;
                    })
                    .OnCancel(op =>
                    {
                        Bambu.Log("Cancelled keyboard", Debugging.ELogCategory.SETTING);
                        onComplete?.Invoke();
                        inputAction.Enable();
                        IsBusy = false;

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

        public float GetAudioVolume(EAudioChannel channel)
        {
            switch (channel)
            {
                case EAudioChannel.MASTER:
                    return AudioMasterSettings.Value;
                case EAudioChannel.SFX:
                    return AudioSFXSettings.Value;
                case EAudioChannel.MUSIC:
                    return AudioMusicSettings.Value;
                case EAudioChannel.UI:
                    return AudioUISettings.Value;
                default:
                    break;
            }

            return 0;
        }

        public void SetAudioVolume(EAudioChannel channel, int value)
        {
            switch (channel)
            {
                case EAudioChannel.MASTER:
                    AudioMasterSettings.Set(value);
                    break;

                case EAudioChannel.SFX:
                    AudioSFXSettings.Set(value);
                    break;

                case EAudioChannel.MUSIC:
                    AudioMusicSettings.Set(value);
                    break;

                case EAudioChannel.UI:
                    AudioUISettings.Set(value);
                    break;

                default:
                    break;
            }
        }
    }
}

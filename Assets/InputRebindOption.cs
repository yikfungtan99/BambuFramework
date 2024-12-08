using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class InputRebindOption : SettingOption
    {
        public override ESettingOptions SettingsOption => ESettingOptions.INPUT_REBIND;

        private SettingsMenu menu;

        private Dictionary<InputAction, Button> actionButtonPairs;

        public override void SpawnUI(SettingsMenu menu, out List<TemplateContainer> templateContainers, out List<Focusable> focussables)
        {
            templateContainers = new List<TemplateContainer>();
            focussables = new List<Focusable>();

            actionButtonPairs = new Dictionary<InputAction, Button>();

            for (int i = 0; i < menu.Player.PlayerInput.actions.actionMaps[0].actions.Count; i++)
            {
                TemplateContainer uiInstance = SettingsUI.CloneTree();

                SetTitle(uiInstance, Title);

                focussables = new List<Focusable>();

                this.menu = menu;

                InputAction inputAction = menu.Player.PlayerInput.actions.actionMaps[0].actions[i];

                if (inputAction.type == InputActionType.Value) continue;
                if (inputAction.name == "Pause") continue;

                Button btnRebind = uiInstance.Q<Button>("btnRebind");

                Title = inputAction.name.ToUpper();
                SetTitle(uiInstance, inputAction.name);

                if (btnRebind != null && inputAction != null)
                {
                    // Set the initial button text to the current binding
                    UpdateRebindButtonText(inputAction, btnRebind);

                    // Add click event for rebinding
                    btnRebind.clicked += () => StartRebinding(inputAction, btnRebind);
                    focussables.Add(btnRebind);
                }

                templateContainers.Add(uiInstance);
                actionButtonPairs.Add(inputAction, btnRebind);

                btnRebind.RegisterCallback<FocusEvent>((e) => Focus(uiInstance));
                btnRebind.RegisterCallback<BlurEvent>((e) => Blur(uiInstance));
            }
        }

        private void StartRebinding(InputAction inputAction, Button rebindButton)
        {
            if (inputAction == null)
                return;

            // Show feedback for rebinding in progress
            rebindButton.text = "Press a key...";

            SettingsManager.Instance.RebindKeys(menu.Player, inputAction, () =>
            {
                UpdateRebindButtonText(inputAction, rebindButton);
            });
        }

        private void UpdateRebindButtonText(InputAction inputAction, Button rebindButton)
        {
            if (inputAction != null)
            {
                string activeControlScheme = PlayerManager.Instance.HostPlayer.CurrentControlScheme;

                int bindingIndex = inputAction.bindings.IndexOf(binding =>
                    activeControlScheme == null || binding.groups.Contains(activeControlScheme));

                if (bindingIndex != -1)
                {
                    //var binding = action.bindings[bindingIndex];
                    //rebindButton.text = InputControlPath.ToHumanReadableString(
                    //    binding.effectivePath,
                    //    InputControlPath.HumanReadableStringOptions.OmitDevice);

                    rebindButton.text = inputAction.GetBindingDisplayString(InputBinding.MaskByGroup(activeControlScheme));
                }
                else
                {
                    rebindButton.text = "Not Bound";
                }
            }
        }

        private string GetActiveControlScheme()
        {
            var user = InputUser.all[0];
            return user.controlScheme?.name ?? "Keyboard&Mouse";
        }

        public override void UpdateSettingOption()
        {
            if (SettingsManager.Instance.IsBusy)
            {
                return;
            }

            foreach (var abp in actionButtonPairs)
            {
                UpdateRebindButtonText(abp.Key, abp.Value);
            }
        }
    }
}
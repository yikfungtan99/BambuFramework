using BambuFramework.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;

namespace BambuFramework.UI
{
    [System.Serializable]
    public class InputRebindOption : SettingOption
    {
        public override ESettingOptions SettingsOption => ESettingOptions.INPUT_REBIND;

        private Button btnRebind;

        private SettingsMenu menu;

        private InputAction inputAction;

        public override TemplateContainer SpawnUI(SettingsMenu menu, out List<Focusable> focussables)
        {
            TemplateContainer uiInstance = base.SpawnUI(menu, out focussables);

            this.menu = menu;

            inputAction = menu.Player.PlayerInput.actions.actionMaps[0].actions[2];

            btnRebind = uiInstance.Q<Button>("btnRebind");

            Title = inputAction.name;
            SetTitle(uiInstance, inputAction.name);

            if (btnRebind != null && inputAction != null)
            {
                // Set the initial button text to the current binding
                UpdateRebindButtonText(btnRebind);

                // Add click event for rebinding
                btnRebind.clicked += () => StartRebinding(btnRebind);
                focussables.Add(btnRebind);
            }

            return uiInstance;
        }

        private void StartRebinding(Button rebindButton)
        {
            if (inputAction == null)
                return;

            // Show feedback for rebinding in progress
            rebindButton.text = "Press a key...";

            SettingsManager.Instance.RebindKeys(menu.Player, inputAction, () =>
            {
                UpdateRebindButtonText(rebindButton);
            });
        }

        private void UpdateRebindButtonText(Button rebindButton)
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

        protected override void Focus(VisualElement v)
        {
            v.AddToClassList("focused");
        }

        protected override void Blur(VisualElement v)
        {
            v.RemoveFromClassList("focused");
        }

        public override void UpdateSettingOption()
        {
            if (SettingsManager.Instance.IsRebinding)
            {
                Debug.Log("REBINDING");
                return;
            }

            base.UpdateSettingOption();

            UpdateRebindButtonText(btnRebind);
        }
    }
}
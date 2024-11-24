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

        [SerializeField] private InputActionReference inputActionReference;

        private Button btnRebind;

        public override TemplateContainer SpawnUI(out List<Focusable> focussables)
        {
            TemplateContainer uiInstance = base.SpawnUI(out focussables);

            btnRebind = uiInstance.Q<Button>("btnRebind");

            if (btnRebind != null && inputActionReference != null)
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
            if (inputActionReference == null || inputActionReference.action == null)
                return;

            var action = inputActionReference.action;

            // Show feedback for rebinding in progress
            rebindButton.text = "Press a key...";
            action.Disable();

            var rebindOperation = action.PerformInteractiveRebinding()
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(op =>
                {
                    // Update the button text to the new binding
                    UpdateRebindButtonText(rebindButton);
                    action.Enable();
                    op.Dispose();
                })
                .Start();
        }

        private void UpdateRebindButtonText(Button rebindButton)
        {
            if (inputActionReference != null && inputActionReference.action != null)
            {
                var action = inputActionReference.action;
                string activeControlScheme = PlayerManager.Instance.HostPlayer.CurrentControlScheme;

                int bindingIndex = action.bindings.IndexOf(binding =>
                    activeControlScheme == null || binding.groups.Contains(activeControlScheme));

                if (bindingIndex != -1)
                {
                    var binding = action.bindings[bindingIndex];
                    rebindButton.text = InputControlPath.ToHumanReadableString(
                        binding.effectivePath,
                        InputControlPath.HumanReadableStringOptions.OmitDevice);
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
            base.UpdateSettingOption();
            UpdateRebindButtonText(btnRebind);
        }
    }
}
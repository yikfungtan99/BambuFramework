using BambuFramework.Debug;
using System;
using UnityEngine.InputSystem;

namespace BambuFramework
{
    public class InputDeviceManager : SingletonBehaviour<InputDeviceManager>
    {
        public event Action<bool> OnGamepadActive; // True for gamepad, false for keyboard/mouse

        private bool isGamepadActive;

        // Public property to check if the gamepad is active
        public bool IsGamepadActive => isGamepadActive;

        private void OnEnable()
        {
            //InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            //InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
            {
                if (device is Gamepad)
                {
                    BambuLogger.Log("Gamepad");
                    isGamepadActive = true;
                    //OnGamepadActive?.Invoke(true);
                }
                else if (device is Keyboard || device is Mouse)
                {
                    isGamepadActive = false;
                    //OnGamepadActive?.Invoke(false);
                }
            }
        }
    }
}

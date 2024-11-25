using UnityEngine;
using UnityEngine.InputSystem;

namespace BambuFramework
{
    public class Player : MonoBehaviour
    {
        private PlayerInput playerInput;
        public PlayerInput PlayerInput { get => playerInput; }

        private InputSystem_Actions inputActions;
        public InputSystem_Actions InputActions => inputActions;
        public string CurrentControlScheme { get; private set; }

        private GameManager gameManager;

        public event System.Action OnInputDeviceChanged;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            inputActions = new InputSystem_Actions();

            playerInput.onControlsChanged += OnControlsChanged;

            // Subscribe to the Pause action
            inputActions.Player.Pause.performed += OnPause;

            // Subscribe to GameManager's events
            gameManager = GameManager.Instance;
            gameManager.OnGameStart += SwitchToGameActionMap;

            // Default to UI action map
            ToggleActionMap(inputActions.UI);
        }

        private void OnControlsChanged(PlayerInput input)
        {
            if (input.currentControlScheme == CurrentControlScheme) return;

            CurrentControlScheme = input.currentControlScheme;
            OnInputDeviceChanged?.Invoke();
        }

        private void OnDestroy()
        {
            if (gameManager != null)
                gameManager.OnGameStart -= SwitchToGameActionMap;

            if (inputActions != null)
                inputActions.Player.Pause.performed -= OnPause;
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            GameManager.Instance.Pause(this);
        }

        public void SwitchToGameActionMap()
        {
            ToggleActionMap(inputActions.Player);
        }

        public void ToggleActionMap(InputActionMap actionMap)
        {
            if (actionMap.enabled)
                return;

            inputActions.Disable();
            actionMap.Enable();
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace BambuFramework
{
    public class Player : MonoBehaviour
    {
        private PlayerInput playerInput;
        public PlayerInput PlayerInput { get => playerInput; }
        public string CurrentControlScheme { get; private set; }

        private GameManager gameManager;

        public event System.Action OnInputDeviceChanged;

        public InputActionMap PlayerActionMap;
        public InputActionMap UiActionMap;
        private InputActionMap currentActionMap;


        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            playerInput.onControlsChanged += OnControlsChanged;

            PlayerActionMap = playerInput.actions.FindActionMap("Player");
            UiActionMap = playerInput.actions.FindActionMap("UI");

            // Subscribe to the Pause action
            PlayerActionMap.FindAction("Pause").performed += OnPause;

            // Subscribe to GameManager's events
            gameManager = GameManager.Instance;
            gameManager.OnGameStart += SwitchToGameActionMap;
            gameManager.OnGameResume += OnResume;

            PlayerActionMap.FindAction("Attack").performed += Attack;

            // Default to UI action map
            ToggleActionMap(UiActionMap);
        }

        private void Attack(InputAction.CallbackContext context)
        {
            Debug.Log("ATTACK");
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

            PlayerActionMap.FindAction("Pause").performed -= OnPause;
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            GameManager.Instance.Pause(this);
            ToggleActionMap(UiActionMap);
        }

        private void OnResume()
        {
            ToggleActionMap(PlayerActionMap);
        }

        public void SwitchToGameActionMap()
        {
            ToggleActionMap(PlayerActionMap);
        }

        public void ToggleActionMap(InputActionMap actionMap)
        {
            if (currentActionMap == null)
            {
                currentActionMap = actionMap;
            }
            else
            {
                if (actionMap == currentActionMap) return;

                currentActionMap.Disable();

                foreach (var item in currentActionMap.actions)
                {
                    item.Disable();
                }
            }

            currentActionMap = actionMap;

            foreach (var item in currentActionMap.actions)
            {
                item.Enable();
            }

            actionMap.Enable();
        }
    }
}

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

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            playerInput.onControlsChanged += OnControlsChanged;

            // Subscribe to the Pause action

            // Subscribe to GameManager's events
            gameManager = GameManager.Instance;
            playerInput.actions["Pause"].performed += OnPause;
            gameManager.OnGameResume += OnResume;
            gameManager.OnGameStart += OnGameStart;
            gameManager.OnGamePaused += OnGamePaused;

            playerInput.actions["Attack"].performed += Attack;

            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("UI").name);
        }

        private void OnResume()
        {
            SwitchToGameActionMap();
        }

        private void OnGameStart()
        {
            SwitchToGameActionMap();
        }

        private void OnGamePaused()
        {
            SwitchToUIActionMap();
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
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            GameManager.Instance.Pause(this);
        }

        private void SwitchToUIActionMap()
        {
            ToggleActionMap("UI");
        }

        public void SwitchToGameActionMap()
        {
            ToggleActionMap("Player");
        }

        public void ToggleActionMap(string actionMap)
        {
            if (playerInput.currentActionMap != null) playerInput.currentActionMap.Disable();
            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap(actionMap).name);
        }
    }
}

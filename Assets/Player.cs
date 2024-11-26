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
            playerInput.actions["Pause"].performed += OnPause;

            // Subscribe to GameManager's events
            gameManager = GameManager.Instance;
            gameManager.OnGameStart += SwitchToGameActionMap;
            gameManager.OnGameResume += OnResume;

            playerInput.actions["Attack"].performed += Attack;

            Debug.Log(playerInput.currentActionMap);

            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("UI").name);

            Debug.Log(playerInput.currentActionMap);
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
            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("UI").name);
        }

        private void OnResume()
        {
        }

        public void SwitchToGameActionMap()
        {
            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("Player").name);
        }

        public void ToggleActionMap(InputActionMap actionMap)
        {

        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace BambuFramework
{
    public class Player : MonoBehaviour
    {
        private InputSystem_Actions inputActions;
        public InputSystem_Actions InputActions { get => inputActions; }

        private GameManager gameManager;


        private void Start()
        {
            inputActions = new InputSystem_Actions();

            inputActions.Player.Pause.performed += OnPause;

            gameManager = GameManager.Instance;
            gameManager.OnGameStart += SwitchToGameActionMap;

            ToggleActionMap(inputActions.UI);
        }

        private void OnDestroy()
        {
            if (gameManager != null) gameManager.OnGameStart -= SwitchToGameActionMap;
        }

        private void Update()
        {

        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void OnChange(PlayerInput input)
        {

        }

        private void OnPause(InputAction.CallbackContext context)
        {
            // Call ShowPause method on UIManager when Pause action is triggered
            GameManager.Instance.Pause(this);
        }

        public void SwitchToGameActionMap()
        {
            ToggleActionMap(inputActions.Player);
        }

        public void ToggleActionMap(InputActionMap actionMap)
        {
            if (actionMap.enabled)
            {
                return;
            }

            inputActions.Disable();
            actionMap.Enable();
        }
    }
}

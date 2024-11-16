using UnityEngine;
using UnityEngine.InputSystem;

namespace BambuFramework
{
    public class Player : MonoBehaviour
    {
        private PlayerInput playerInput;

        public PlayerInput PlayerInput { get => playerInput; }

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            playerInput.onControlsChanged += OnChange;
        }

        private void OnDisable()
        {
            playerInput.onControlsChanged -= OnChange;
        }

        private void OnChange(PlayerInput input)
        {

        }
    }
}

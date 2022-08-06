using SunsetSystems.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        public delegate void MovementVectorChangedHandler(Vector2 movementVector);
        public static event MovementVectorChangedHandler MovementVectorChanged;

        private GameManager gameManager;
        private PlayerInput playerInput;

        private void Start()
        {
            if (!gameManager)
                gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
            if (!gameManager)
                Debug.LogError(gameObject.name + " >> no Game Manager found in scene! " + gameObject.GetInstanceID());
            if (!playerInput)
                playerInput = GetComponent<PlayerInput>();
        }

        public void OnMovePaddle(InputAction.CallbackContext context)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            MovementVectorChanged?.Invoke(movementVector);
        }

        public void OnLaunchBall(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (!gameManager.HasGameStarted)
            {
                gameManager.StartGame();
            }
        }

        public void OnSwitchPauseState(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (!gameManager.IsGamePaused)
            {
                gameManager.PauseGame();
                playerInput.DeactivateInput();
            }
            else
            {
                gameManager.ResumeGame();
                playerInput.ActivateInput();
            }
        }
    }
}

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
            EnsureDependencies();
        }

        private void EnsureDependencies()
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
            if (GameManager.IsGamePaused)
                return;
            Vector2 movementVector = context.ReadValue<Vector2>();
            MovementVectorChanged?.Invoke(movementVector);
        }

        public void OnLaunchBall(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            EnsureDependencies();
            if (!GameManager.HasGameStarted && !GameManager.IsGamePaused)
            {
                gameManager.StartGame();
            }
        }

        public void OnSwitchPauseState(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            EnsureDependencies();
            if (!GameManager.IsGamePaused)
            {
                gameManager.PauseGame();
            }
            else
            {
                gameManager.ResumeGame();
            }
        }
    }
}

using SunsetSystems.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orkanoid.Core
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public delegate void MovementVectorChangedHandler(Vector2 movementVector);
        public static event MovementVectorChangedHandler MovementVectorChanged;

        private GameManager gameManager;

        private void Start()
        {
            if (!gameManager)
                gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
            if (!gameManager)
                Debug.LogError(gameObject.name + " >> no Game Manager found in scene! " + gameObject.GetInstanceID());
        }

        public void OnMovePaddle(InputAction.CallbackContext context)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            Debug.Log(movementVector);
            MovementVectorChanged?.Invoke(movementVector);
        }

        public void OnLaunchBall(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            gameManager.StartGame();
        }

        public void OnPauseGame(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            Debug.Log("Pause game");
        }
    }
}

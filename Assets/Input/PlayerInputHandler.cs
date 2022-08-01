using UnityEngine;
using UnityEngine.InputSystem;

namespace Orkanoid.Core
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public delegate void MovementVectorChangedHandler(Vector2 movementVector);
        public static event MovementVectorChangedHandler MovementVectorChanged;

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
            Debug.Log("Launch ball");
        }

        public void OnPauseGame(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            Debug.Log("Pause game");
        }
    }
}

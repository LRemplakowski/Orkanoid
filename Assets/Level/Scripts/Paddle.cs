using Orkanoid.Core;
using UnityEngine;

namespace Orkanoid.Level
{
    public class Paddle : MonoBehaviour
    {
        private const float MOVEMENT_SPEED_BASE = 1.0f;
        [SerializeField]
        private float movementSpeedMultiplier = 1.0f, acceleration = 1.0f, currentSpeed;
        private float MaxPaddleSpeed => movementSpeedMultiplier * MOVEMENT_SPEED_BASE;
        [SerializeField]
        private Vector2 cachedMovementVector = Vector2.zero;

        private void OnEnable()
        {
            PlayerInputHandler.MovementVectorChanged += OnMovementVectorChanged;
        }

        private void OnDisable()
        {
            PlayerInputHandler.MovementVectorChanged -= OnMovementVectorChanged;
        }

        private void OnMovementVectorChanged(Vector2 movementVector)
        {
            cachedMovementVector = new Vector2(movementVector.x, 0f);
            currentSpeed = 0f;
        }

        private void FixedUpdate()
        {
            currentSpeed = Mathf.Lerp(0f, MaxPaddleSpeed, (currentSpeed / MaxPaddleSpeed) + (acceleration * Time.fixedDeltaTime * Mathf.Abs(cachedMovementVector.x)));
            transform.Translate(cachedMovementVector * currentSpeed);
        }
    }
}

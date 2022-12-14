using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    [RequireComponent(typeof(Tagger))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Paddle : MonoBehaviour
    {
        private const float MOVEMENT_SPEED_BASE = 1.0f;
        [SerializeField]
        private float movementSpeedMultiplier = 1.0f, acceleration = 1.0f;
        private float currentSpeed = 0.0f;
        private float MaxPaddleSpeed => movementSpeedMultiplier * MOVEMENT_SPEED_BASE;
        private Vector2 cachedMovementVector = Vector2.zero;
        public Vector2 CurrentMovementVector => cachedMovementVector * currentSpeed;
        [SerializeField]
        private GameObject ballHookPoint;
        public Vector3 BallHookPointPosition => ballHookPoint != null ? ballHookPoint.transform.position : transform.position;
        private Vector3 originalHookPointPosition;

        [SerializeField]
        private Rigidbody2D myRigidbody;
        [SerializeField]
        private SpriteRenderer myRenderer;
        [SerializeField]
        private Vector2 movementConstraintBox = Vector2.one;

        private static Vector3 _originalPaddleScale;
        public static Vector3 OriginalPaddleScale { get => _originalPaddleScale; }
        private static Vector3 _currentPaddleScale;
        public static Vector3 CurrentPaddleScale { get => _currentPaddleScale; }
        private static Vector3 _originalPosition;
        public static Vector3 OriginalPosition { get => _originalPosition; }

        [SerializeField]
        private int powerUpRangeMin = -2, powerUpRangeMax = 2;
        private static int _powerUpCounter = 0;
        public static int PowerUpCounter { get => _powerUpCounter; }
        [SerializeField, Range(0f, 1f)]
        private float paddleWidthAdjust = 0.5f;

        private void Awake()
        {
            if (!myRigidbody)
                myRigidbody = GetComponent<Rigidbody2D>();
            if (!myRenderer)
                myRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _originalPaddleScale = transform.localScale;
            _currentPaddleScale = _originalPaddleScale;
            originalHookPointPosition = ballHookPoint.transform.localPosition;
            _originalPosition = transform.position;
        }

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
            transform.position = (ConstrainPosition((Vector2)transform.position + CurrentMovementVector));
        }

        private Vector2 ConstrainPosition(Vector2 position)
        {
            Vector2 result = position;
            float maxX, minX;
            maxX = (movementConstraintBox.x / 2) - (myRenderer.sprite.bounds.size.x / 2 * transform.localScale.x);
            minX = -maxX;
            result = position.x > maxX ? new Vector2(maxX, position.y) : result;
            result = position.x < minX ? new Vector2(minX, position.y) : result;
            return result;
        }

        public void OverridePaddleWidth(int powerUpCounter)
        {
            _powerUpCounter = powerUpCounter;
            _currentPaddleScale = new(OriginalPaddleScale.x + (paddleWidthAdjust * _powerUpCounter), OriginalPaddleScale.y, OriginalPaddleScale.z);
            transform.localScale = CurrentPaddleScale;
        }

        public void IncreasePaddleWidth()
        {
            if (_powerUpCounter >= powerUpRangeMax)
                return;
            _powerUpCounter++;
            _currentPaddleScale = new(OriginalPaddleScale.x + (paddleWidthAdjust * _powerUpCounter), OriginalPaddleScale.y, OriginalPaddleScale.z);
            transform.localScale = CurrentPaddleScale;
        }

        public void DecreasePaddleWidth()
        {
            if (_powerUpCounter <= powerUpRangeMin)
                return;
            _powerUpCounter--;
            _currentPaddleScale = new(OriginalPaddleScale.x + (paddleWidthAdjust * _powerUpCounter), OriginalPaddleScale.y, OriginalPaddleScale.z);
            transform.localScale = CurrentPaddleScale;
        }

        public void ResetPaddle()
        {
            transform.localScale = _originalPaddleScale;
            transform.position = _originalPosition;
            ballHookPoint.transform.localPosition = originalHookPointPosition;
            _powerUpCounter = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(0f, transform.position.y, -1f), movementConstraintBox);
        }
    }
}

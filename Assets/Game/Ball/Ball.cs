using Orkanoid.Core;
using SunsetSystems.Utils;
using System;
using UnityEngine;

namespace Orkanoid.Game
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        private GameManager gameManager;
        private Paddle paddle;
        private Rigidbody2D myRigidbody;
        private AudioSource audioSource;

        [SerializeField]
        private float launchVelocity = 1.0f;
        private static int _currentBallDamage;
        public static int CurrentBallDamage { get => _currentBallDamage; }

        private delegate void BallResizeHandler();
        private static event BallResizeHandler BallResized;

        private static Vector3 _currentBallScale;
        public static Vector3 CurrentBallScale { get => _currentBallScale; }
        private static Vector3 _originalBallScale;
        public static Vector3 OriginalBallScale { get => _originalBallScale; }

        private Vector2 cachedBallVelocity;

        private static readonly int defaultBallDamage = 1;
        private static readonly int powerUpRangeMin = -2;
        private static readonly int powerUpRangeMax = 2;
        private static int _powerUpCounter = 0;
        public static int PowerUpCounter { get => _powerUpCounter; }
        private static readonly float ballWidthAdjust = 0.2f;

        private void Awake()
        {
            if (!myRigidbody)
                myRigidbody = GetComponent<Rigidbody2D>();
            if (!audioSource)
                audioSource = GetComponent<AudioSource>();
            _originalBallScale = transform.localScale;
            _currentBallScale = _originalBallScale;
            _currentBallDamage = defaultBallDamage;
        }

        private void OnEnable()
        {
            GameManager.GameStarted += LaunchBall;
            BallResized += OnBallResized;
            GameManager.GamePaused += OnGamePaused;
            GameManager.GameResumed += OnGameResumed;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void OnDisable()
        {
            GameManager.GameStarted -= LaunchBall;
            BallResized -= OnBallResized;
            GameManager.GamePaused -= OnGamePaused;
            GameManager.GameResumed -= OnGameResumed;
        }

        private void OnBallResized()
        {
            transform.localScale = _currentBallScale;
        }

        private void OnGamePaused()
        {
            cachedBallVelocity = myRigidbody.velocity;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void OnGameResumed()
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            myRigidbody.velocity = cachedBallVelocity;
        }

        private void Start()
        {
            if (!gameManager)
                gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
            if (!gameManager)
                Debug.LogError(gameObject.name + " >> Game Manager not found! " + gameObject.GetInstanceID());
            if (!paddle)
                paddle = this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE);
            if (!paddle)
                Debug.LogError(gameObject.name + " >> Paddle object not found! " + gameObject.GetInstanceID());
        }

        private void Update()
        {
            if (!GameManager.HasGameStarted)
                transform.position = paddle.BallHookPointPosition;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IBrick brick))
            {
                brick.TakeHit(_currentBallDamage);
            }
            if (GameManager.HasGameStarted)
            {
                if (!SoundController.MuteSounds)
                    audioSource.Play();
                Vector2 tweak = new(UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(0f, 0.05f));
                myRigidbody.velocity += tweak;
            }
        }

        public void LaunchBall()
        {
            Vector2 launchVector;
            if (Mathf.Approximately(0f, paddle.CurrentMovementVector.x))
            {
                launchVector = Vector2.up + Vector2.right;
            }
            else
            {
                launchVector = Vector2.up + paddle.CurrentMovementVector;
            }
            launchVector.Normalize();
            myRigidbody.velocity = launchVector * launchVelocity;
        }

        public static void OverrideBallSize(int ballPowerUp)
        {
            _powerUpCounter = ballPowerUp;
            _currentBallScale += new Vector3(PowerUpCounter * ballWidthAdjust, PowerUpCounter * ballWidthAdjust, PowerUpCounter * ballWidthAdjust);
            BallResized?.Invoke();
        }

        public static void IncreaseBallSize()
        {
            if (_powerUpCounter >= powerUpRangeMax)
                return;
            _powerUpCounter++;
            _currentBallScale += new Vector3(PowerUpCounter * ballWidthAdjust, PowerUpCounter * ballWidthAdjust, PowerUpCounter * ballWidthAdjust);
            BallResized?.Invoke();
        }

        public static void DecreaseBallSize()
        {
            if (_powerUpCounter <= powerUpRangeMin)
                return;
            _powerUpCounter++;
            _currentBallScale += new Vector3(PowerUpCounter * ballWidthAdjust, PowerUpCounter * ballWidthAdjust, PowerUpCounter * ballWidthAdjust);
            BallResized?.Invoke();
        }

        public static void ResetBallSize()
        {
            _currentBallScale = _originalBallScale;
            _powerUpCounter = 0;
            BallResized?.Invoke();
        }

        public static void AdjustBallDamage()
        {
            _currentBallDamage = Mathf.Clamp(defaultBallDamage + _powerUpCounter, 1, powerUpRangeMax);
        }
    }
}


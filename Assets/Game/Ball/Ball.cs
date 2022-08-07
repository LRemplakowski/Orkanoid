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
        [SerializeField]
        private int defaultBallDamage = 1;
        private static int currentBallDamage;

        private delegate void BallResizeHandler();
        private static event BallResizeHandler BallResized;

        private static Vector3 ballScale;
        private static Vector3 originalScale;

        private void Awake()
        {
            if (!myRigidbody)
                myRigidbody = GetComponent<Rigidbody2D>();
            if (!audioSource)
                audioSource = GetComponent<AudioSource>();
            originalScale = transform.localScale;
            ballScale = originalScale;
            currentBallDamage = defaultBallDamage;
        }

        private void OnEnable()
        {
            GameManager.GameStarted += LaunchBall;
            BallResized += OnBallResized;
        }

        private void OnDisable()
        {
            GameManager.GameStarted -= LaunchBall;
            BallResized -= OnBallResized;
        }

        private void OnBallResized()
        {
            transform.localScale = ballScale;
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
            if (!gameManager.HasGameStarted)
                transform.position = paddle.BallHookPointPosition;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IBrick brick))
            {
                brick.TakeHit(currentBallDamage);
            }
            if (gameManager.HasGameStarted)
            {
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
            Debug.LogWarning("Launching ball with velocity " + (launchVector * launchVelocity).ToString());
            myRigidbody.velocity = launchVector * launchVelocity;
        }

        public static void ResizeBalls(float multiplier)
        {
            ballScale *= multiplier;
            BallResized?.Invoke();
        }

        public static void ResetBallSize()
        {
            ballScale = originalScale;
            BallResized?.Invoke();
        }

        public static void AdjustBallDamage(int modifier)
        {
            currentBallDamage = Mathf.Clamp(currentBallDamage + modifier, 1, int.MaxValue);
            Debug.Log("Adjusted ball damage: " + currentBallDamage);
        }
    }
}


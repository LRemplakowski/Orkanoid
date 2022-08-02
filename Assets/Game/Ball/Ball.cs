using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        private GameManager gameManager;
        private Paddle paddle;
        private Rigidbody2D myRigidbody;

        private bool lockToPaddle = true;

        [SerializeField]
        private float launchForceMultiplier = 1.0f;

        private void Awake()
        {
            if (!myRigidbody)
                myRigidbody = GetComponent<Rigidbody2D>();
            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            lockToPaddle = true;
        }

        private void OnEnable()
        {
            GameManager.GameStarted += LaunchBall;
        }

        private void OnDisable()
        {
            GameManager.GameStarted -= LaunchBall;
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

        public void LaunchBall()
        {
            lockToPaddle = false;
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
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
            myRigidbody.AddForce(launchVector * launchForceMultiplier);
        }

        private void FixedUpdate()
        {
            if (lockToPaddle)
            {
                myRigidbody.MovePosition(paddle.BallHookPointPosition);
            }
        }
    }
}


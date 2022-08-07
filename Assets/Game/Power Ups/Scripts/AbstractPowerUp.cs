using Orkanoid.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AbstractPowerUp : MonoBehaviour, IPowerUp
    {
        [SerializeField]
        private float fallingSpeed = 1f;

        private void Awake()
        {
            Rigidbody2D myRigidbody = GetComponent<Rigidbody2D>();
            myRigidbody.velocity = Vector2.down * fallingSpeed;
        }

        private void OnEnable()
        {
            AbstractBrick.AllBricksSmashed += KillMe;
        }

        private void OnDisable()
        {
            AbstractBrick.AllBricksSmashed -= KillMe;
        }

        private void KillMe()
        {
            Destroy(gameObject);
        }

        public abstract void Apply();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Paddle _))
            {
                Apply();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out HealthLossArea _))
                Destroy(gameObject);
        }
    }
}


using Orkanoid.Core;
using SunsetSystems.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Tagger))]
    public abstract class AbstractBrick : MonoBehaviour, IBrick
    {
        [SerializeField]
        private string ID = "BRICK";

        [SerializeField]
        protected int maxHealth, hitsTaken = 0;
        [SerializeField]
        protected SpriteRenderer spriteRenderer;
        [SerializeField]
        protected Collider2D brickCollider;

        protected static BrickPool brickPool;
        protected static GameManager gameManager;

        public delegate void BrickDestroyedHandler(AbstractBrick brick);
        public event BrickDestroyedHandler BrickDestroyed;
        public delegate void HitTakenHandler(AbstractBrick brick);
        public event HitTakenHandler HitTaken;

        protected virtual void Awake()
        {
            if (!spriteRenderer)
                if (!TryGetComponent(out spriteRenderer))
                    Debug.LogError(gameObject.name + " >> Sprite Renderer not found! " + gameObject.GetInstanceID());
            if (!brickCollider)
                if (!TryGetComponent(out brickCollider))
                    Debug.LogError(gameObject.name + " >> Collider2D not found! " + gameObject.GetInstanceID());
        }

        protected void OnEnable()
        {
            BrickDestroyed += OnHealthBelowZero;
            HitTaken += OnHitTaken;
        }

        protected void OnDisable()
        {
            BrickDestroyed -= OnHealthBelowZero;
            HitTaken -= OnHitTaken;
        }

        protected virtual void Start()
        {
            if (!brickPool)
                brickPool = this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL);
            if (!gameManager)
                gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
        }

        public virtual int GetHealthLeft()
        {
            return maxHealth - hitsTaken;
        }

        public virtual int GetPointValue()
        {
            return maxHealth;
        }

        public virtual void TakeHit(int damage)
        {
            hitsTaken += damage;
            HitTaken?.Invoke(this);
            if (hitsTaken >= maxHealth)
                BrickDestroyed?.Invoke(this);
        }

        public Transform GetTransform() => transform;

        public GameObject GetGameObject() => gameObject;

        public abstract BrickType GetBrickType();

        protected abstract void OnHealthBelowZero(IBrick brick);

        protected abstract void OnHitTaken(IBrick brick);

        public virtual void ResetBrick()
        {
            hitsTaken = 0;
        }

        public string GetBrickID()
        {
            return ID;
        }
    }
}

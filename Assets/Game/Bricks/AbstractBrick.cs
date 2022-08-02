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
        protected int maxHealth, hitsTaken = 0;
        [SerializeField]
        protected List<Sprite> sprites = new();
        [SerializeField]
        protected SpriteRenderer spriteRenderer;
        [SerializeField]
        protected Collider2D brickCollider;

        private int ScoreBase => this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER).GameScoreBase;

        private delegate void BrickDestroyedHandler(AbstractBrick brick);
        private event BrickDestroyedHandler BrickDestroyed;
        private delegate void HitTakenHandler(AbstractBrick brick);
        private event HitTakenHandler HitTaken;

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
            try
            {
                spriteRenderer.sprite = sprites[hitsTaken];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
                spriteRenderer.sprite = Sprite.Create(Texture2D.blackTexture, new(), Vector2.zero);
            }
        }

        public virtual int GetHealthLeft()
        {
            return maxHealth - hitsTaken;
        }

        public virtual int GetPointValue()
        {
            return maxHealth * ScoreBase;
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

        protected abstract void OnHealthBelowZero(AbstractBrick brick);

        protected abstract void OnHitTaken(AbstractBrick brick);
    }
}

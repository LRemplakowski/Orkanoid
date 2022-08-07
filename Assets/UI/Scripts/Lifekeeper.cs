using Orkanoid.Core;
using TMPro;
using UnityEngine;

namespace Orkanoid.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Lifekeeper : MonoBehaviour
    {
        [SerializeField]
        private GameObject heartParent;
        [SerializeField]
        private GameObject heartPrefab;

        private void OnEnable()
        {
            GameManager.LifeCountChanged += OnLifeCountChanged;
        }

        private void OnDisable()
        {
            GameManager.LifeCountChanged -= OnLifeCountChanged;
        }

        private void OnLifeCountChanged(int currentLives)
        {
            heartParent.transform.DestroyChildren();
            for (int i = 0; i < currentLives; i++)
            {
                Instantiate(heartPrefab, heartParent.transform);
            }
        }
    }
}

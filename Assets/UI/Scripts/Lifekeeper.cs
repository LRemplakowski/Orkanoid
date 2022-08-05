using Orkanoid.Core;
using TMPro;
using UnityEngine;

namespace Orkanoid.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Lifekeeper : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        private void Awake()
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();
        }

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
            text.text = "Lives: " + currentLives;
        }
    }
}

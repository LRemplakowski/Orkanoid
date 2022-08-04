using Orkanoid.Core;
using TMPro;
using UnityEngine;

namespace Orkanoid.UI
{
    public class Scorekeeper : MonoBehaviour
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
            GameManager.ScoreChanged += OnScoreChanged;
        }

        private void OnDisable()
        {
            GameManager.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int currentScore)
        {
            text.text = "Score: " + currentScore;
        }
    }
}

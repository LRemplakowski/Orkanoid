using Orkanoid.Core;
using TMPro;
using UnityEngine;

namespace Orkanoid.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HighScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        private void OnEnable()
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();
            DisplayScore();
            GameManager.ScoreChanged += OnScoreChanged;
        }

        private void OnDisable()
        {
            GameManager.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int currentScore)
        {
            DisplayScore();
        }

        private void DisplayScore()
        {
            int score = GameManager.CurrentHighScore;
            string textToDisplay = "High score: " + score;
            text.text = textToDisplay;
        }
    }
}

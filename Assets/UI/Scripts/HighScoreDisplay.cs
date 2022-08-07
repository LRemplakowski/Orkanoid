using Orkanoid.Core;
using System.Collections;
using System.Collections.Generic;
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
            int score = GameManager.GetHighScore();
            string textToDisplay = "Your highest score: \n" + score;
            text.text = textToDisplay;
        }
    }
}

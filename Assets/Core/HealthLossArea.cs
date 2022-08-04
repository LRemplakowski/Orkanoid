using Orkanoid.Game;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Core
{
    public class HealthLossArea : MonoBehaviour
    {
        private GameManager gameManager;

        private void Start()
        {
            gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Ball _))
            {
                gameManager.LoseLife();
            }
        }
    }
}

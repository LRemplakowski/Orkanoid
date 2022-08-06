using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.UI
{
    public class NewGame : MonoBehaviour
    {
        private Canvas mainMenuCanvas;
        private GameManager gameManager;

        public async void DoStartNewGame()
        {
            EnsureDependencies();
            gameManager.ResetGame();
            await gameManager.NextLevel(0, () => mainMenuCanvas.gameObject.SetActive(false));

            void EnsureDependencies()
            {
                if (!mainMenuCanvas)
                    mainMenuCanvas = this.FindFirstComponentWithTag<Canvas>(TagConstants.MAIN_MENU_CANVAS);
                if (!gameManager)
                    gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
            }
        }
    }
}

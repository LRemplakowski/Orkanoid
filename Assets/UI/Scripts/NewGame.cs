using Orkanoid.Core;
using Orkanoid.Game;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.UI
{
    public class NewGame : MonoBehaviour
    {
        private Canvas mainMenuCanvas;
        private GameManager gameManager;
        private PlaySpace playSpace;

        public async void DoStartNewGame()
        {
            EnsureDependencies();
            gameManager.ResetGame();
            await gameManager.NextLevel(0, () => DisableMenuEnablePlayspace(mainMenuCanvas, playSpace));

            void DisableMenuEnablePlayspace(Canvas mainMenu, PlaySpace playSpace)
            {
                mainMenu.gameObject.SetActive(false);
                playSpace.gameObject.SetActive(true);
            }

            void EnsureDependencies()
            {
                if (!mainMenuCanvas)
                    mainMenuCanvas = this.FindFirstComponentWithTag<Canvas>(TagConstants.MAIN_MENU_CANVAS);
                if (!gameManager)
                    gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
                if (!playSpace)
                    playSpace = this.FindFirstComponentWithTag<PlaySpace>(TagConstants.PLAY_SPACE);
            }
        }
    }
}

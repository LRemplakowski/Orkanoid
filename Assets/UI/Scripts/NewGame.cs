using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.UI
{
    public class NewGame : MonoBehaviour
    {
        private FadePanel fadePanel;
        private Canvas mainMenuCanvas;
        private GameManager gameManager;

        public async void DoStartNewGame()
        {
            EnsureDependencies();
            await fadePanel.FadeOut();
            mainMenuCanvas.gameObject.SetActive(false);
            await gameManager.NextLevel(0);
            await fadePanel.FadeIn();


            void EnsureDependencies()
            {
                if (!fadePanel)
                    fadePanel = this.FindFirstComponentWithTag<FadePanel>(TagConstants.FADE_PANEL);
                if (!mainMenuCanvas)
                    mainMenuCanvas = this.FindFirstComponentWithTag<Canvas>(TagConstants.MAIN_MENU_CANVAS);
                if (!gameManager)
                    gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
            }
        }
    }
}

using Orkanoid.Core;
using SunsetSystems.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitToMenu : MonoBehaviour
{
    public async void ExitGame()
    {
        GameManager gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
        await gameManager.ReturnToMainMenu();
        gameManager.ResetGame();
    }
}

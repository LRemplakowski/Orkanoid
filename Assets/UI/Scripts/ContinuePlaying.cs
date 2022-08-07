using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;

public class ContinuePlaying : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
        gameManager.ResumeGame();
    }
}

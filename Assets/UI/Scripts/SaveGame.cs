using Orkanoid.Core.Saves;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.UI
{
    public class SaveGame : MonoBehaviour
    {
        public void DoSaveGame()
        {
            SaveLoadManager saveLoadManager = this.FindFirstComponentWithTag<SaveLoadManager>(TagConstants.SAVE_LOAD_MANAGER);
            saveLoadManager.SaveGame();
        }
    }
}

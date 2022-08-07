using Orkanoid.Core.Saves;
using SunsetSystems.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Orkanoid.UI
{
    [RequireComponent(typeof(Button))]
    public class ContinueSave : MonoBehaviour
    {
        [SerializeField]
        private SaveLoadManager saveLoadManager;
        [SerializeField]
        private Button button;

        // Start is called before the first frame update
        private void Start()
        {
            if (!saveLoadManager)
                saveLoadManager = this.FindFirstComponentWithTag<SaveLoadManager>(TagConstants.SAVE_LOAD_MANAGER);
            if (!button)
                button = GetComponent<Button>();
            button.interactable = saveLoadManager.SaveFileExists();
        }

        public void LoadGame()
        {
            saveLoadManager.LoadGame();
        }
    }
}

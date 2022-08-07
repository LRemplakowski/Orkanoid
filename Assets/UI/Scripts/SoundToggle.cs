using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Orkanoid.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class SoundToggle : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Image image;
        [SerializeField]
        private Sprite soundOnIcon, soundOffIcon;

        private void Awake()
        {
            if (!button)
                button = GetComponent<Button>();
            if (!image)
                image = GetComponent<Image>();
        }

        private void Start()
        {
            image.sprite = SoundController.MuteSounds ? soundOffIcon : soundOnIcon;
        }

        public void ToggleSound()
        {
            SoundController soundController = this.FindFirstComponentWithTag<SoundController>(TagConstants.SOUND_CONTROLLER);
            soundController.SetSoundMuted(!SoundController.MuteSounds);
            image.sprite = SoundController.MuteSounds ? soundOffIcon : soundOnIcon;
        }
    }
}

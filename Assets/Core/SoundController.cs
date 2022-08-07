using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Tagger))]
    public class SoundController : MonoBehaviour
    {
        private const string SOUND_MUTED = "SOUND_MUTED";

        [SerializeField]
        private AudioSource audioSource;

        private static bool _muteSounds = false;
        public static bool MuteSounds
        {
            get
            {
                return _muteSounds;
            }
            set
            {
                _muteSounds = value;
                PlayerPrefs.SetInt(SOUND_MUTED, _muteSounds ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        private void Awake()
        {
            _muteSounds = PlayerPrefs.GetInt(SOUND_MUTED) == 1;
            if (!audioSource)
                audioSource = GetComponent<AudioSource>();
            audioSource.mute = MuteSounds;
        }

        public void SetSoundMuted(bool soundMuted)
        {
            MuteSounds = soundMuted;
            audioSource.mute = MuteSounds;
        }
    }
}

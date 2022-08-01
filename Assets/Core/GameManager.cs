using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(Tagger))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _gameScoreBase = 100;
        public int GameScoreBase { get => _gameScoreBase; }

    }
}

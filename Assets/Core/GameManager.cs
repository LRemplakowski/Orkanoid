using SunsetSystems.Utils;
using System;
using UnityEngine;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(Tagger))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _gameScoreBase = 100;
        public int GameScoreBase => _gameScoreBase;

        [SerializeField]
        private bool _hasGameStarted = false;
        public bool HasGameStarted => _hasGameStarted;

        public delegate void GameStartedHandler();
        public static event GameStartedHandler GameStarted;
        public delegate void GameStoppedHandler();
        public static event GameStoppedHandler GameStopped;

        public void StartGame()
        {
            _hasGameStarted = true;
            GameStarted?.Invoke();
        }

        public void StopGame()
        {
            _hasGameStarted = false;
            GameStopped?.Invoke();
        }
    }
}

using Orkanoid.Core.Levels;
using Orkanoid.UI;
using SunsetSystems.Utils;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(Tagger))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _currentScore = 0;
        public int CurrentScore
        {
            get
            {
                return _currentLives;
            }
            private set
            {
                _currentScore = value;
                ScoreChanged?.Invoke(_currentScore);
            }
        }
        [SerializeField]
        private int _currentLives = 3;
        public int CurrentLives
        {
            get
            {
                return _currentLives;
            }
            private set
            {
                _currentLives = value;
                LifeCountChanged?.Invoke(_currentLives);
            }
        }
        [SerializeField]
        private int _currentLevel = 0;
        public int CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            private set
            {
                _currentLevel = value;
                LevelChanged?.Invoke(_currentLevel);
            }
        }

        [SerializeField]
        private int gameScoreBase = 100;

        [field: SerializeField]
        public bool HasGameStarted { get; private set; }
        [field: SerializeField]
        public bool IsGamePaused { get; private set; }

        public delegate void GameStartedHandler();
        public static event GameStartedHandler GameStarted;

        public delegate void GameStoppedHandler();
        public static event GameStoppedHandler GameStopped;

        public delegate void GamePausedHandler();
        public static event GamePausedHandler GamePaused;

        public delegate void GameResumedHandler();
        public static event GameResumedHandler GameResumed;

        public delegate void ScoreChangedHandler(int currentScore);
        public static event ScoreChangedHandler ScoreChanged;

        public delegate void LifeCountChangedHandler(int currentLives);
        public static event LifeCountChangedHandler LifeCountChanged;

        public delegate void LevelChangedHandler(int currentLevel);
        public static event LevelChangedHandler LevelChanged;

        [SerializeField]
        private LevelLoader levelLoader;
        [SerializeField]
        private FadePanel fadePanel;

        private void Start()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            ScoreChanged?.Invoke(_currentScore);
            LifeCountChanged?.Invoke(_currentLives);
        }

        public void StartGame()
        {
            HasGameStarted = true;
            GameStarted?.Invoke();
        }

        public void StopGame()
        {
            HasGameStarted = false;
            GameStopped?.Invoke();
        }

        public void PauseGame()
        {
            IsGamePaused = true;
            GamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            IsGamePaused = false;
            GameResumed?.Invoke();
        }

        public void LoseLife()
        {
            CurrentLives--;
            if (CurrentLives <= 0)
                GameOver();
            else
                StopGame();
        }

        public void AddPoints(int pointValue)
        {
            // Let's say we win a game on integer overflow in score.
            // Can change this to long overflow if needed.
            try
            {
                CurrentScore = checked(_currentScore + (gameScoreBase * pointValue));
            }
            catch (OverflowException)
            {
                CurrentScore = int.MaxValue;
                GameOver();
            }
        }

        public async Task NextLevel()
        {
            await NextLevel(CurrentLevel + 1, null);
        }

        public async Task NextLevel(Action actionOnFade)
        {
            await NextLevel(CurrentLevel + 1, actionOnFade);
        }

        public async Task NextLevel(int levelIndex)
        {
            await NextLevel(levelIndex, null);
        }

        public async Task NextLevel(int levelIndex, Action actionOnFade)
        {
            EnsureDependencies();
            CurrentLevel = levelIndex;
            StopGame();
            await fadePanel.FadeOut();
            actionOnFade?.Invoke();
            await levelLoader.NextLevel(CurrentLevel);
            await fadePanel.FadeIn();
            ResumeGame();

            void EnsureDependencies()
            {
                if (!levelLoader)
                    levelLoader = this.FindFirstComponentWithTag<LevelLoader>(TagConstants.LEVEL_LOADER);
                if (!fadePanel)
                    fadePanel = this.FindFirstComponentWithTag<FadePanel>(TagConstants.FADE_PANEL);
            }
        }

        private void GameOver()
        {
            CurrentLives = 3;
            StopGame();
        }
    }
}

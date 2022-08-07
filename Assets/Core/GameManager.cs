using Orkanoid.Core.Levels;
using Orkanoid.Core.Saves;
using Orkanoid.Game;
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
        private const string HIGHEST_SCORE = "HIGHEST_SCORE";

        private static int _currentScore = 0;
        public static int CurrentScore
        {
            get
            {
                return _currentScore;
            }
            private set
            {
                _currentScore = value;
                ScoreChanged?.Invoke(_currentScore);
            }
        }
        private static int _currentLives = 3;
        public static int CurrentLives
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

        private static int _currentLevel = 0;
        public static int CurrentLevel
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
        private static int _currentHighScore = 0;
        public static int CurrentHighScore
        {
            get
            {
                if (_currentHighScore <= 0)
                    _currentHighScore = GetSavedHighScore();
                if (CurrentScore > _currentHighScore)
                    _currentHighScore = CurrentScore;
                return _currentHighScore;
            }
        }

        [SerializeField]
        private int gameScoreBase = 100;

        public static bool HasGameStarted { get; private set; }
        public static bool IsGamePaused { get; private set; }

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

        private void OnEnable()
        {
            AbstractBrick.AllBricksSmashed += OnAllBricksDestroyed;
        }

        private void OnDisable()
        {
            AbstractBrick.AllBricksSmashed -= OnAllBricksDestroyed;
        }

        public void ResetGame()
        {
            _currentScore = 0;
            _currentLives = 3;
            _currentLevel = 0;
            ScoreChanged?.Invoke(_currentScore);
            LifeCountChanged?.Invoke(_currentLives);
            StopGame();
            ResumeGame();
            Paddle paddle = this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE);
            paddle.ResetPaddle();
            Ball.ResetBallSize();
            Ball.AdjustBallDamage(int.MinValue);
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
            this.TryFindFirstWithTag(TagConstants.PAUSE_GAME_CANVAS, out GameObject pauseUI);
            pauseUI.SetActive(true);
        }

        public void ResumeGame()
        {
            IsGamePaused = false;
            GameResumed?.Invoke();
            this.TryFindFirstWithTag(TagConstants.PAUSE_GAME_CANVAS, out GameObject pauseUI);
            pauseUI.SetActive(false);
        }

        public void LoseLife()
        {
            CurrentLives--;
            if (CurrentLives <= 0)
                GameOver();
            else
                StopGame();
        }

        public void AddLives(int lives)
        {
            CurrentLives += lives;
        }

        public void AddPoints(int pointValue)
        {
            // Let's say we win a game on integer overflow in score.
            // Can change this to long overflow if needed.
            try
            {
                CurrentScore = checked(_currentScore + (gameScoreBase * pointValue * (CurrentLevel + 1)));
            }
            catch (OverflowException)
            {
                CurrentScore = int.MaxValue;
                GameOver();
            }
        }

        private async void OnAllBricksDestroyed()
        {
            if (!IsGamePaused && CurrentLives > 0)
                await NextLevel();
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
            Ball.ResetBallSize();
            this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE).ResetPaddle();
            await levelLoader.NextLevel(CurrentLevel);
            await fadePanel.FadeIn();
        }

        internal async Task LoadSavedLevel(SaveData savedData)
        {
            EnsureDependencies();
            ResetGame();
            CurrentLevel = savedData.levelID;
            CurrentLives = savedData.currentLives;
            CurrentScore = savedData.currentScore;
            await fadePanel.FadeOut();
            SwitchUIToGame();
            InjectBallData(savedData);
            InjectPaddleData(savedData);
            levelLoader.LoadSavedLevel(savedData.bricks);
            await fadePanel.FadeIn();

            void InjectBallData(SaveData savedData)
            {
                Ball.ResetBallSize();
                Ball.AdjustBallDamage(int.MinValue);
                Ball.AdjustBallDamage(savedData.ballDamage);
                Ball.ResizeBalls(savedData.ballScale);
            }

            void SwitchUIToGame()
            {
                this.FindFirstComponentWithTag<PlaySpace>(TagConstants.PLAY_SPACE).gameObject.SetActive(true);
                this.FindFirstComponentWithTag<Canvas>(TagConstants.MAIN_MENU_CANVAS).gameObject.SetActive(false);
            }

            void InjectPaddleData(SaveData savedData)
            {
                Paddle paddle = this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE);
                paddle.ResetPaddle();
                paddle.AdjustHookPointPosition(savedData.ballScale);
                paddle.ResizePaddle(savedData.paddleScale);
            }
        }

        private void EnsureDependencies()
        {
            if (!levelLoader)
                levelLoader = this.FindFirstComponentWithTag<LevelLoader>(TagConstants.LEVEL_LOADER);
            if (!fadePanel)
                fadePanel = this.FindFirstComponentWithTag<FadePanel>(TagConstants.FADE_PANEL);
        }

        private async void GameOver()
        {
            SaveHighScore();
            StopGame();
            await ReturnToMainMenu();
        }

        public async Task ReturnToMainMenu()
        {
            await fadePanel.FadeOut();
            this.FindFirstComponentWithTag<Canvas>(TagConstants.MAIN_MENU_CANVAS).gameObject.SetActive(true);
            this.FindFirstComponentWithTag<PlaySpace>(TagConstants.PLAY_SPACE).gameObject.SetActive(false);
            await fadePanel.FadeIn();
        }

        private void SaveHighScore()
        {
            int savedScore = PlayerPrefs.GetInt(HIGHEST_SCORE, 0);
            if (CurrentHighScore > savedScore)
            {
                PlayerPrefs.SetInt(HIGHEST_SCORE, CurrentHighScore);
                PlayerPrefs.Save();
            }
        }

        private static int GetSavedHighScore()
        {
            return PlayerPrefs.GetInt(HIGHEST_SCORE, 0);
        }
    }
}

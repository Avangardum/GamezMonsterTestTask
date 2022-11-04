using System;
using System.Globalization;
using UnityEngine;

namespace Avangardum.GamezMonsterTestTask
{
    public class UiPresenter : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private StartMenu _startMenu;
        [SerializeField] private GameOverMenu _gameOverMenu;

        private void Awake()
        {
            _startMenu.Difficulty = _gameManager.Difficulty;
            _startMenu.DifficultyChanged += StartMenuOnDifficultyChanged;
            _startMenu.StartClicked += OnStartClicked;

            _gameOverMenu.RestartClicked += OnStartClicked;
            _gameOverMenu.ChangeDifficultyClicked += GameOverMenuOnChangeDifficultyClicked;

            _gameManager.GameOver += OnGameOver;
        }

        private void StartMenuOnDifficultyChanged(object sender, int difficulty)
        {
            _gameManager.Difficulty = difficulty;
        }

        private void GameOverMenuOnChangeDifficultyClicked(object sender, EventArgs e)
        {
            _gameOverMenu.gameObject.SetActive(false);
            _startMenu.gameObject.SetActive(true);
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            _startMenu.gameObject.SetActive(false);
            _gameOverMenu.gameObject.SetActive(false);
            _gameManager.StartGame();
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            _gameOverMenu.gameObject.SetActive(true);
            _gameOverMenu.TotalTries = _gameManager.TotalTries.ToString();
            _gameOverMenu.SurvivalTime = _gameManager.SurvivalTime.ToString("0.0", CultureInfo.InvariantCulture);
        }
    }
}
using System;
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
        }

        private void StartMenuOnDifficultyChanged(object sender, int difficulty)
        {
            _gameManager.Difficulty = difficulty;
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            _startMenu.gameObject.SetActive(false);
            _gameOverMenu.gameObject.SetActive(false);
            _gameManager.StartGame();
        }
    }
}
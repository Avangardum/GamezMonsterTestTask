using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avangardum.GamezMonsterTestTask
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _survivalTimeValueText;
        [SerializeField] private TextMeshProUGUI _totalTriesValueText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _changeDifficultyButton;

        public event EventHandler RestartClicked;
        public event EventHandler ChangeDifficultyClicked;

        public string SurvivalTime
        {
            set => _survivalTimeValueText.text = value;
        }

        public string TotalTries
        {
            set => _totalTriesValueText.text = value;
        }

        private void Awake()
        {
            _restartButton.onClick.AddListener(() => RestartClicked?.Invoke(this, EventArgs.Empty));
            _changeDifficultyButton.onClick.AddListener(() => ChangeDifficultyClicked?.Invoke(this, EventArgs.Empty));
        }
    }
}
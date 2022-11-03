using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avangardum.GamezMonsterTestTask
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private List<Button> _difficultyButtons;
        
        private int _selectedDifficultyIndex;

        public event EventHandler StartClicked;
        public event EventHandler<int> SelectedDifficultyIndexChanged;
        
        public int SelectedDifficultyIndex
        {
            get => _selectedDifficultyIndex;
            set
            {
                _selectedDifficultyIndex = value;
                for (int i = 0; i < _difficultyButtons.Count; i++)
                {
                    _difficultyButtons[i].interactable = i != _selectedDifficultyIndex;
                }
                SelectedDifficultyIndexChanged?.Invoke(this, _selectedDifficultyIndex);
            }
        }

        private void Awake()
        {
            _startButton.onClick.AddListener(() => StartClicked?.Invoke(this, EventArgs.Empty));
            for (int i = 0; i < _difficultyButtons.Count; i++)
            {
                var index = i;
                _difficultyButtons[i].onClick.AddListener(() => SelectedDifficultyIndex = index);
            }
        }
    }
}
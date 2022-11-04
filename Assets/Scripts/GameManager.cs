using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Avangardum.GamezMonsterTestTask
{
    public class GameManager : MonoBehaviour
    {
        // How much level should always be generated in front of the player
        private const float GenerationRange = 10;
        private const float CleanupRange = 20;
        private const float CleanupInterval = 5;
        private const int DefaultDifficulty = 1;
        private const string DifficultyPlayerPrefsKey = "Difficulty";
        private const string TotalTriesCountPlayerPrefsKey = "Total Tries";
        
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _boundsSectionPrefab;
        [SerializeField] private float _boundsSectionLength;
        [SerializeField] private GameObject _obstaclePrefab;
        [Header("Config")] 
        [SerializeField] private List<float> _horizontalSpeedByDifficulty;
        [SerializeField] private float _baseVerticalSpeed;
        [SerializeField] private float _verticalSpeedGrowthInterval;
        [SerializeField] private float _verticalSpeedGrowthAmount;
        [SerializeField] private float _minObstacleY;
        [SerializeField] private float _maxObstacleY;
        [SerializeField] private float _obstacleXInterval;

        private GameObject _player;
        private Rigidbody2D _playerRigidbody;
        private readonly List<GameObject> _levelObjects = new();
        private float _lastGenerationEndX;
        private float _nextBoundsSectionX;
        private float _nextObstacleX;
        private float _currentVerticalSpeed;
        private float _currentHorizontalSpeed;
        private Coroutine _increaseVerticalSpeedCoroutine;

        public Vector2 PlayerPosition => _player.transform.position;
        
        public bool IsGameActive { get; private set; }

        public int Difficulty
        {
            get => PlayerPrefs.GetInt(DifficultyPlayerPrefsKey, DefaultDifficulty);
            set
            {
                PlayerPrefs.SetInt(DifficultyPlayerPrefsKey, value);
                PlayerPrefs.Save();
            }
        }

        private void Start()
        {
            StartCoroutine(CleanupCoroutine());
        }

        private void Update()
        {
            if (!IsGameActive) return;

            var isLevelGenerationRequired = _player.transform.position.x + GenerationRange > _lastGenerationEndX;
            if (isLevelGenerationRequired)
            {
                GenerateLevel();
            }
        }

        private void FixedUpdate()
        {
            if (!IsGameActive) return;
            
            MovePlayer();
        }

        private void MovePlayer()
        {
            var signedVerticalSpeed = _inputManager.IsUpPressed ? _currentVerticalSpeed : -_currentVerticalSpeed;
            var velocity = new Vector2(_currentHorizontalSpeed, signedVerticalSpeed);
            var movement = velocity * Time.deltaTime;
            _playerRigidbody.MovePosition(_playerRigidbody.position + movement);
        }

        public void StartGame()
        {
            // Cleanup
            if (_player != null)
            {
                Destroy(_player);
            }
            _levelObjects.ForEach(Destroy);

            // Set variables
            _lastGenerationEndX = -GenerationRange;
            _nextObstacleX = _obstacleXInterval;
            _nextBoundsSectionX = -GenerationRange;
            _currentVerticalSpeed = _baseVerticalSpeed;
            _currentHorizontalSpeed = _horizontalSpeedByDifficulty[Difficulty];

            // Initialize player
            _player = Instantiate(_playerPrefab);
            _playerRigidbody = _player.GetComponent<Rigidbody2D>();
            _player.GetComponent<CollisionDetector>().TriggerEntered += OnPlayerCollision;

            GenerateLevel();

            // Restart the vertical speed increase coroutine
            if (_increaseVerticalSpeedCoroutine != null)
            {
                StopCoroutine(_increaseVerticalSpeedCoroutine);
            }
            _increaseVerticalSpeedCoroutine = StartCoroutine(IncreaseVerticalSpeedCoroutine());

            IsGameActive = true;
        }

        private void GenerateLevel()
        {
            float generationEndX = _player.transform.position.x + GenerationRange * 2;
            
            // Generate bounds
            while (_nextBoundsSectionX <= generationEndX)
            {
                var boundsSection = Instantiate(_boundsSectionPrefab);
                boundsSection.transform.position = Vector3.right * _nextBoundsSectionX;
                _levelObjects.Add(boundsSection);
                _nextBoundsSectionX += _boundsSectionLength;
            }

            // Generate obstacles
            while (_nextObstacleX <= generationEndX)
            {
                var obstacle = Instantiate(_obstaclePrefab);
                obstacle.transform.position = new Vector3(_nextObstacleX, Random.Range(_minObstacleY, _maxObstacleY));
                _levelObjects.Add(obstacle);
                _nextObstacleX += _obstacleXInterval;
            }

            _lastGenerationEndX = generationEndX;
        }

        private IEnumerator CleanupCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(CleanupInterval);
                while (IsCleanupRequired())
                {
                    Destroy(_levelObjects[0]);
                    _levelObjects.RemoveAt(0);
                }
            }
            
            bool IsCleanupRequired() => _levelObjects.Any() && 
                _levelObjects[0].transform.position.x < _player.transform.position.x - CleanupRange;
        }

        private IEnumerator IncreaseVerticalSpeedCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_verticalSpeedGrowthInterval);
                _currentVerticalSpeed += _verticalSpeedGrowthAmount;
            }
        }
        
        private void OnPlayerCollision(object sender, Collider2D collider)
        {
            IsGameActive = false;
        }
    }
}

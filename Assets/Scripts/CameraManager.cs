using System;
using UnityEngine;

namespace Avangardum.GamezMonsterTestTask
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;

        private float _xOffset;

        private void Awake()
        {
            _xOffset = transform.position.x;
        }

        private void LateUpdate()
        {
            if (!_gameManager.IsGameActive) return;

            transform.position = 
                new Vector3(_gameManager.PlayerPosition.x + _xOffset, transform.position.y, transform.position.z);
        }
    }
}
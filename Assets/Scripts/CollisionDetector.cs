using System;
using UnityEngine;

namespace Avangardum.GamezMonsterTestTask
{
    public class CollisionDetector : MonoBehaviour
    {
        public event EventHandler<Collider2D> TriggerEntered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEntered?.Invoke(this, other);
        }
    }
}
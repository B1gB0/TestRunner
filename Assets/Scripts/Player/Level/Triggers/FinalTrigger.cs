using System;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class FinalTrigger : Trigger
    {
        public event Action OnVictory;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var _))
            {
                OnVictory?.Invoke();
                Deactivate();
            }
        }
    }
}
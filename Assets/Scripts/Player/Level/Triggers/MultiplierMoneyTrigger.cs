using System;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class MultiplierMoneyTrigger : Trigger
    {
        public event Action OnIncreaseMoney;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var _))
            {
                OnIncreaseMoney?.Invoke();
                Deactivate();
            }
        }
    }
}
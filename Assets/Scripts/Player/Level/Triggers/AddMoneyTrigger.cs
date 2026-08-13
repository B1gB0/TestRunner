using System;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class AddMoneyTrigger : Trigger
    {
        [SerializeField] private int moneyAmount;
        
        public event Action<int> OnGetMoney;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var player))
            {
                OnGetMoney?.Invoke(moneyAmount);
                player.Health.AddHealth(moneyAmount);
                Deactivate();
            }
        }
    }
}
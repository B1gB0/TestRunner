using System;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class SpendMoneyTrigger : Trigger
    {
        [SerializeField] private int moneyAmount;
        
        public event Action<int> OnSpendMoney;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var player))
            {
                OnSpendMoney?.Invoke(moneyAmount);
                player.Health.TakeDamage(moneyAmount);
                Deactivate();
            }
        }
    }
}
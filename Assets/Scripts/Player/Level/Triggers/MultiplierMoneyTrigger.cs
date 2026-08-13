using System;
using TMPro;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class MultiplierMoneyTrigger : Trigger
    {
        [SerializeField] private int _multiplier;
        [SerializeField] private TextMeshPro _text;
        
        public event Action<int> OnIncreaseMoney;

        private void Start()
        {
            _text.text = _multiplier.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var _))
            {
                OnIncreaseMoney?.Invoke(_multiplier);
                Deactivate();
            }
        }
    }
}
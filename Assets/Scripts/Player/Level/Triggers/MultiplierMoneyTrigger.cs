using System;
using TMPro;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class MultiplierMoneyTrigger : Trigger
    {
        [SerializeField] private int _multiplier;
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Animator _animator;
        
        public event Action<int> OnIncreaseMoney;

        private void Start()
        {
            _text.text = _multiplier + "x";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var _))
            {
                OnIncreaseMoney?.Invoke(_multiplier);
                _animator.CrossFade("Open", 0.5f);
            }
        }
    }
}
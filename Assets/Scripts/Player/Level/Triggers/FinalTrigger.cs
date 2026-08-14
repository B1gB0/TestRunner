using System;
using Player.State;
using UnityEngine;

namespace Player.Level.Triggers
{
    public class FinalTrigger : Trigger
    {
        public event Action OnVictory;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var player))
            {
                OnVictory?.Invoke();
                player.StateMachine.SwitchState(StateId.Dance);
                Deactivate();
            }
        }
    }
}
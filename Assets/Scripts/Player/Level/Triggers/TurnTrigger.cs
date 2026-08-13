using System;
using Player.Level.Triggers;
using UnityEngine;

public class TurnTrigger : Trigger
{
    [SerializeField] private float turnAngle = 90f;
    
    public event Action<float> OnTurn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player.Core.Player>(out var _))
        {
            OnTurn?.Invoke(turnAngle);
            Deactivate();
        }
    }
}
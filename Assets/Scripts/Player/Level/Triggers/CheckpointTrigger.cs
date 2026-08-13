using UnityEngine;

namespace Player.Level.Triggers
{
    public class CheckpointTrigger : Trigger
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var _))
            {
                
            }
        }
    }
}
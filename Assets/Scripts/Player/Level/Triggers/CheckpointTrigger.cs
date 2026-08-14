using UnityEngine;

namespace Player.Level.Triggers
{
    public class CheckpointTrigger : Trigger
    {
        [SerializeField] private Animator _animator;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Core.Player>(out var _))
            {
                _animator.CrossFade("Rise", 1f);
            }
        }
    }
}
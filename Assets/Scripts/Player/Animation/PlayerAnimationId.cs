using UnityEngine;

namespace Player.Animation
{
    public class PlayerAnimationId
    {
        public readonly int Idle = Animator.StringToHash(nameof(Idle));
        public readonly int Run = Animator.StringToHash(nameof(Run));
    }
}
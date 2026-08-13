using UnityEngine;

namespace Player.Animation
{
    public class PlayerAnimatedState
    {
        protected readonly Animator Animator;
        protected readonly PlayerAnimationId PlayerAnimationId;

        public PlayerAnimatedState(Animator animator)
        {
            Animator = animator;
            PlayerAnimationId = new PlayerAnimationId();
        }

        public void OnMove(float speed)
        {
            Animator.SetFloat(PlayerAnimationId.Run, speed);
        }
    }
}
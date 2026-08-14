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
            Animator.CrossFade(PlayerAnimationId.Run, 0.2f);
        }
        
        public void OnDance(float speed)
        {
            Animator.CrossFade(PlayerAnimationId.Dance, 0.2f);
        }
        
        public void OnIdle(float speed)
        {
            Animator.CrossFade(PlayerAnimationId.Idle, 0.2f);
        }
    }
}
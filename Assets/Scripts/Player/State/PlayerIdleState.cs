using Player.Core;

namespace Player.State
{
    public class PlayerIdleState : IPlayerState
    {
        private readonly Core.Player _player;
        private readonly PlayerStateMachine _stateMachine;

        public PlayerIdleState(Core.Player player)
        {
            _player = player;
            _stateMachine = _player.StateMachine;
        }

        public StateId IdState => StateId.Idle;

        public void Enter()
        {
            _player.PlayerAnimatedState.OnMove(0f);
        }

        public void Update()
        {
            if (_player.InputController.IsMoveInputPerformed)
            {
                _stateMachine.SwitchState(StateId.Move);
            }
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
        }
    }
}
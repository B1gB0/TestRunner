using Player.Core;

namespace Player.State
{
    public class PlayerDanceState : IPlayerState
    {
        private readonly Core.Player _player;
        private readonly PlayerStateMachine _stateMachine;

        public PlayerDanceState(Core.Player player)
        {
            _player = player;
            _stateMachine = _player.StateMachine;
        }

        public StateId IdState => StateId.Idle;

        public void Enter()
        {
            _player.PlayerAnimatedState.OnDance(0f);
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Exit()
        {
        }
    }
}
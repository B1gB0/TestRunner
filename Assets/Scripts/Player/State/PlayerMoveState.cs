using Player.Animation;
using Player.Core;
using UnityEngine;

namespace Player.State
{
    public class PlayerMoveState : IPlayerState
    {
        private const float StrafeFactor = 0.5f;
        private const float StrafeSmoothTime = 0.1f;
        private const float StrafeMaxSpeed = 10f;

        private readonly Core.Player _player;
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerAnimatedState _playerAnimatedState;

        private bool _isTurning;
        private float _targetYaw;
        private float _turnSpeed = 180f; // градусов в секунду

        private float _currentStrafeX;
        private float _strafeVelocity;

        public PlayerMoveState(Core.Player player)
        {
            _player = player;
            _stateMachine = _player.StateMachine;
            _playerAnimatedState = _player.PlayerAnimatedState;
        }

        public StateId IdState => StateId.Move;

        public void Enter()
        {
            _playerAnimatedState.OnMove(1f);
            _currentStrafeX = 0f;
            _strafeVelocity = 0f;
        }

        public void Update() { }

        public void FixedUpdate()
        {
            if (_isTurning)
                HandleTurning();

            Vector3 moveDirection = _player.transform.forward;
            moveDirection.y = 0;
            moveDirection.Normalize();

            float strafeInput = _player.InputController.MoveDirection.x;
            
            _currentStrafeX = Mathf.SmoothDamp(
                _currentStrafeX,
                strafeInput,
                ref _strafeVelocity,
                StrafeSmoothTime,
                StrafeMaxSpeed,
                Time.fixedDeltaTime
            );

            Vector3 strafeVector = _player.transform.right * _currentStrafeX;
            Vector3 finalDirection = (moveDirection + strafeVector * StrafeFactor).normalized;

            float speed = _player.PlayerCharacteristics.MoveSpeed;
            Vector3 moveDelta = finalDirection * speed * Time.fixedDeltaTime;
            _player.transform.position += moveDelta;
        }

        public void StartTurn(float angleDelta)
        {
            _targetYaw = _player.transform.eulerAngles.y + angleDelta;
            _isTurning = true;
        }

        public void Exit()
        {
            _isTurning = false;
            _currentStrafeX = 0f;
            _strafeVelocity = 0f;
        }

        private void HandleTurning()
        {
            float currentYaw = _player.transform.eulerAngles.y;
            float newYaw = Mathf.MoveTowardsAngle(currentYaw, _targetYaw, _turnSpeed * Time.fixedDeltaTime);
            _player.transform.rotation = Quaternion.Euler(0, newYaw, 0);

            if (Mathf.Abs(Mathf.DeltaAngle(currentYaw, _targetYaw)) < 0.1f)
                _isTurning = false;
        }
    }
}
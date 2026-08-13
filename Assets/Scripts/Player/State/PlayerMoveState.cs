using Player.Animation;
using Player.Core;
using UnityEngine;

namespace Player.State
{
    public class PlayerMoveState : IPlayerState
    {
        private const float StrafeFactor = 0.5f;
        
        private readonly Core.Player _player;
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerAnimatedState _playerAnimatedState;
        
        private bool _isTurning = false;
        private float _targetYaw;
        private float _turnSpeed = 180f;

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
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            if (_isTurning)
            {
                HandleTurning();
            }

            Vector3 moveDirection = _player.transform.forward;
            moveDirection.y = 0;
            moveDirection.Normalize();
            
            float strafeInput = _player.InputController.MoveDirection.x;
            Vector3 strafeVector = _player.transform.right * strafeInput;
            
            Vector3 finalDirection = (moveDirection + strafeVector * StrafeFactor).normalized;
            
            Vector3 velocity = finalDirection * _player.PlayerCharacteristics.MoveSpeed;
            
            velocity = ApplyGroundProjection(velocity);

            _player.Rigidbody.velocity = velocity;
            
            float currentSpeed = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z).magnitude;
            _playerAnimatedState.OnMove(currentSpeed / _player.PlayerCharacteristics.MoveSpeed);
        }
        
        public void StartTurn(float angleDelta)
        {
            _targetYaw = _player.transform.eulerAngles.y + angleDelta;
            _isTurning = true;
        }

        public void Exit()
        {
            _isTurning = false;
        }

        private void HandleTurning()
        {
            float currentYaw = _player.transform.eulerAngles.y;
            float newYaw = Mathf.MoveTowardsAngle(currentYaw, _targetYaw, _turnSpeed * Time.fixedDeltaTime);
            _player.transform.rotation = Quaternion.Euler(0, newYaw, 0);

            if (Mathf.Abs(Mathf.DeltaAngle(currentYaw, _targetYaw)) < 0.1f)
            {
                _isTurning = false;
            }
        }

        private Vector3 ApplyGroundProjection(Vector3 velocity)
        {
            float rayLength = 1f;
            Vector3 rayStart = _player.transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength))
            {
                velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
                if (velocity.y < 0)
                    velocity.y -= 2f;
            }
            else
            {
                velocity.y = _player.Rigidbody.velocity.y;
            }
            return velocity;
        }
    }
}
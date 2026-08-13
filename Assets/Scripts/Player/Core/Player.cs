using Effects;
using Player.Animation;
using Player.Characteristics;
using Player.Input;
using Player.State;
using Services;
using UnityEngine;

namespace Player.Core
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public PlayerCollisionHandler PlayerCollisionHandler { get; private set; }

        private ParticleEffectsService _particleEffectsService;

        private Animator _animator;
        private Rigidbody _rigidbody;
        private InputController _inputController;

        private PlayerStateMachine _stateMachine;
        private PlayerAnimatedState _playerAnimatedState;

        private PlayerIdleState _playerIdleState;
        private PlayerMoveState _playerMoveState;

        public Animator Animator => _animator;
        public Rigidbody Rigidbody => _rigidbody;

        public PlayerStateMachine StateMachine => _stateMachine;
        public InputController InputController => _inputController;
        public PlayerAnimatedState PlayerAnimatedState => _playerAnimatedState;

        public PlayerCharacteristics PlayerCharacteristics { get; private set; }

        public bool CanFollow { get; private set; } = true;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (Health == null)
                return;
            
            Health.Die += Die;
            Health.IsDamaged += OnPlayHitEffect;
        }

        private void OnDisable()
        {
            if (Health == null)
                return;
            
            Health.Die -= Die;
            Health.IsDamaged -= OnPlayHitEffect;
        }

        private void OnDestroy()
        {
            if (PlayerCharacteristics != null && Health != null)
                Health.TargetHealthChanged -= PlayerCharacteristics.SaveTargetHealth;
            
            _inputController.OnUnlockController -= OnUnlockController;
        }

        public void Construct(
            PlayerCharacteristics playerCharacteristics,
            ParticleEffectsService particleEffectsService)
        {
            PlayerCharacteristics = playerCharacteristics;
            _particleEffectsService = particleEffectsService;

            if (Health != null && PlayerCharacteristics != null)
                Health.TargetHealthChanged += PlayerCharacteristics.SaveTargetHealth;
        }

        public void StartTurn(float angle)
        {
            _playerMoveState.StartTurn(angle);
        }

        private void OnPlayHitEffect()
        {
            if (_particleEffectsService != null && Health != null && Health.HitPoint != null)
                _particleEffectsService.PlayEffect(ParticleType.RedBloodHit, Health.HitPoint.position);
        }

        private void Initialize()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _inputController = GetComponent<InputController>();
            _stateMachine = GetComponent<PlayerStateMachine>();

            _stateMachine.Initialize(this);

            _playerAnimatedState = new PlayerAnimatedState(Animator);

            _playerIdleState = new PlayerIdleState(this);
            _playerMoveState = new PlayerMoveState(this);

            _stateMachine.AddState(_playerIdleState);
            _stateMachine.AddState(_playerMoveState);

            _stateMachine.SwitchState(StateId.Idle);

            _inputController.OnUnlockController += OnUnlockController;
        }

        private void OnUnlockController()
        {
            _stateMachine.SwitchState(StateId.Idle);
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}
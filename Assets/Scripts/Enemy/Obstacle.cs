using System;
using DataBase.Data;
using Effects;
using Enemy.StateMachine.Animation;
using Enemy.StateMachine.Behaviour;
using Player.Experience;
using Services;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Obstacle : MonoBehaviour, IAcceptable, IExperienceScoreActor
    {
        [SerializeField] private Animator _animator;

        protected IFloatingTextService FloatingTextService;
        protected AudioSoundsService AudioSoundsService;
        protected ParticleEffectsService ParticleEffectsService;
        protected IExperiencePoints ExperiencePoints;
        protected ICurrencyService CurrencyService;
        
        private bool _isDead;

        public event Action<Obstacle> Die;
        
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        
        public bool IsEnemy { get; private set; }

        public ObstacleData Data { get; private set; }
        public Player.Core.Player Player { get; private set; }
        public EnemyAnimatedStateMachine AnimatedStateMachine { get; private set; }
        public ObstacleType Type { get; private set; }

        private void Start()
        {
            AnimatedStateMachine = new EnemyAnimatedStateMachine(_animator);
        }

        private void OnEnable()
        {
            // Health.Die += OnDie;
            // Health.IsDamaged += OnPlayHitEffect;
        }

        private void OnDisable()
        {
            // Health.Die -= OnDie;
            // Health.IsDamaged -= OnPlayHitEffect;
        }

        public void Construct(
            Player.Core.Player player,
            ObstacleData obstacleData,
            IFloatingTextService floatingTextService,
            ParticleEffectsService particleEffectsService,
            AudioSoundsService audioSoundsService,
            ICurrencyService currencyService)
        {
            Player = player;
            Data = obstacleData;
            Type = Data.Type;
            
            IsEnemy = true;

            FloatingTextService = floatingTextService;
            ParticleEffectsService = particleEffectsService;
            AudioSoundsService = audioSoundsService;
            CurrencyService = currencyService;
        }

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
            CurrencyService.AddMoney(Data.Money);
        }

        public virtual void OnReactState(bool isEnteredToState)
        {
        }

        protected virtual void OnDie()
        {
            if (_isDead) return;
            _isDead = true;

            Die?.Invoke(this);
        }

        protected virtual void OnPlayHitEffect()
        {
            ParticleEffectsService.PlayEffect(ParticleType.BasicHit, transform.position);
        }
    }
}
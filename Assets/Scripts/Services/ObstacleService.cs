using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataBase.Data;
using DataBase.InitDataSO;
using Enemy;
using Reflex.Attributes;
using UnityEngine;

namespace Services
{
    public class ObstacleService : IObstacleService
    {
        private const string MoneyPool = nameof(MoneyPool);
        private const string BottlePool = nameof(BottlePool);

        private const bool IsAutoExpand = true;
        private const int MinValue = 0;
        private const int DefaultCountObjectsInPool = 3;

        private readonly Dictionary<ObstacleType, ObstacleData> _obstaclesData = new();

        private IDataBaseService _dataBaseService;
        private IPlayerService _playerService;
        private IFloatingTextService _floatingTextService;
        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;
        // private IExperiencePoints _experiencePoints;
        private ICurrencyService _currencyService;
        private ITweenAnimationService _tweenAnimationService;

        private ObstacleInitData _obstacleInitData;
        
        private ObjectPool<Bottle> _bottlePool;
        private ObjectPool<Money> _moneyPool;

        public bool IsInitiated { get; private set; }

        [Inject]
        public void Construct(
            IDataBaseService dataBaseService,
            IPlayerService playerService,
            AudioSoundsService audioSoundsService,
            ParticleEffectsService particleEffectsService,
            IFloatingTextService floatingTextService,
            // IExperiencePoints experiencePoints,
            ICurrencyService currencyService,
            ITweenAnimationService tweenAnimationService)
        {
            _dataBaseService = dataBaseService;
            _playerService = playerService;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
            _floatingTextService = floatingTextService;
            // _experiencePoints = experiencePoints;
            _currencyService = currencyService;
            _tweenAnimationService = tweenAnimationService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            foreach (var enemy in _dataBaseService.Content.Obstacles)
            {
                _obstaclesData.TryAdd(enemy.Type, enemy);
            }

            IsInitiated = true;

            return UniTask.CompletedTask;
        }
        
        public void GetData(ObstacleInitData obstacleInitData)
        {
            _obstacleInitData = obstacleInitData;
        }
        
        public Bottle CreateBottle()
        {
            CreateBottlePool();

            var data = _obstaclesData[ObstacleType.Bottle];
            var bottle = _bottlePool.GetFreeElement();

            bottle.Construct(
                _playerService.Player,
                data,
                _floatingTextService,
                _particleEffectsService,
                _audioSoundsService,
                _currencyService,
                _tweenAnimationService);

            return bottle;
        }
        
        public Money CreateMoney()
        {
            CreateMoneyPool();

            var data = _obstaclesData[ObstacleType.Money];
            var money = _moneyPool.GetFreeElement();

            money.Construct(
                _playerService.Player,
                data,
                _floatingTextService,
                _particleEffectsService,
                _audioSoundsService,
                _currencyService,
                _tweenAnimationService);

            return money;
        }
        
        private void CreateBottlePool()
        {
            if (_bottlePool != null)
                return;
            
            var poolParent = new GameObject(BottlePool);
            Object.DontDestroyOnLoad(poolParent);

            _bottlePool = new ObjectPool<Bottle>(
                _obstacleInitData.BottlePrefab,
                DefaultCountObjectsInPool,
                poolParent.transform.transform)
            {
                AutoExpand = IsAutoExpand,
            };
        }
        
        private void CreateMoneyPool()
        {
            if (_moneyPool != null)
                return;
            
            var poolParent = new GameObject(MoneyPool);
            Object.DontDestroyOnLoad(poolParent);

            _moneyPool = new ObjectPool<Money>(
                _obstacleInitData.MoneyPrefab,
                DefaultCountObjectsInPool,
                poolParent.transform)
            {
                AutoExpand = IsAutoExpand,
            };
        }
    }
}
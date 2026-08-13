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
        private const string SkeletonPool = nameof(SkeletonPool);
        private const string SkeletonHeavyArmorPool = nameof(SkeletonHeavyArmorPool);
        private const string SkeletonRangerPool = nameof(SkeletonRangerPool);
        private const string PriestPool = nameof(PriestPool);
        private const string BanditPool = nameof(BanditPool);
        private const string BanditRangerPool = nameof(BanditRangerPool);
        private const string ArrowProjectilePool = nameof(ArrowProjectilePool);
        private const string MagicBallProjectilePool = nameof(MagicBallProjectilePool);

        private const bool IsAutoExpand = true;
        private const int MinValue = 0;
        private const int DefaultCountObjectsInPool = 3;

        private readonly Dictionary<ObstacleType, ObstacleData> _enemiesData = new();

        private IDataBaseService _dataBaseService;
        private IPlayerService _playerService;
        private IFloatingTextService _floatingTextService;
        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;
        // private IExperiencePoints _experiencePoints;
        private ICurrencyService _currencyService;

        private EnemyInitData _enemyInitData;

        public bool IsInitiated { get; private set; }

        [Inject]
        public void Construct(
            IDataBaseService dataBaseService,
            IPlayerService playerService,
            AudioSoundsService audioSoundsService,
            ParticleEffectsService particleEffectsService,
            IFloatingTextService floatingTextService,
            // IExperiencePoints experiencePoints,
            ICurrencyService currencyService)
        {
            _dataBaseService = dataBaseService;
            _playerService = playerService;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
            _floatingTextService = floatingTextService;
            // _experiencePoints = experiencePoints;
            _currencyService = currencyService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            foreach (var enemy in _dataBaseService.Content.Obstacles)
            {
                _enemiesData.TryAdd(enemy.Type, enemy);
            }

            IsInitiated = true;

            return UniTask.CompletedTask;
        }
        
        public void GetData(EnemyInitData enemyInitData)
        {
            _enemyInitData = enemyInitData;
        }
    }
}
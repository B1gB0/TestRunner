using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DataBase.InitDataSO;
using Game.Constant;
using Game.GameRoot;
using Player.Core;
using Player.Level.Spawners;
using Player.Level.Triggers;
using Reflex.Attributes;
using Services;
using UI.StateMachine;
using UI.View;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Level
{
    public abstract class Level : MonoBehaviour
    {
        protected const float MinValue = 0f;

        protected ViewFactory ViewFactory;
        protected UIStateMachine UIStateMachine;
        protected UIRootView UIRootView;

        protected float LastSpawnTime;

        protected ObstacleSpawner ObstacleSpawner;
        
        [SerializeField] private List<TurnTrigger>  _turnTriggers;
        [SerializeField] private List<AddMoneyTrigger>  _addMoneyTriggers;
        [SerializeField] private List<SpendMoneyTrigger>  _spendMoneyTriggers;

        private IObstacleService _obstacleService;
        private IPlayerService _playerService;
        private ParticleEffectsService _particleEffectsService;
        private AudioSoundsService _audioSoundsService;

        private LevelInitData _levelInitData;
        private PlayerInitData _playerInitData;
        private CinemachineFreeLook _cinemachineFreeLook;
        private Core.Player _player;

        public event Action IsInitiatedSpawners;
        public event Action PlayerIsSpawned;
        public event Action OnGoToNextScene;

        public HealthBar HealthBar { get; private set; }

        [Inject]
        private void Construct(
            IObstacleService obstacleService,
            IPlayerService playerService,
            ParticleEffectsService particleEffectsService,
            AudioSoundsService audioSoundsService)
        {
            _obstacleService = obstacleService;
            _playerService = playerService;
            _particleEffectsService = particleEffectsService;
            _audioSoundsService = audioSoundsService;
        }

        private void OnDestroy()
        {
            foreach (var turnTrigger in _turnTriggers)
            {
                turnTrigger.OnTurn -= _player.StartTurn;
            }
            
            _player.Health.HealthChanged -= HandleHealthChanges;
        }

        public void GetDependencies(
            LevelInitData levelInitData,
            PlayerInitData playerInitData,
            CinemachineFreeLook cinemachineFreeLook,
            ViewFactory viewFactory,
            UIStateMachine uiStateMachine,
            UIRootView uiRootView
        )
        {
            _levelInitData = levelInitData;
            _playerInitData = playerInitData;
            _cinemachineFreeLook = cinemachineFreeLook;

            ViewFactory = viewFactory;
            UIStateMachine = uiStateMachine;
            UIRootView = uiRootView;
        }

        public virtual async UniTask OnStartLevel()
        {
            InitSpawners(_obstacleService);
            
            ObstacleSpawner.SpawnObstacles(_levelInitData);
            
            await CreatePlayer();
        }

        protected async UniTask CreatePlayer()
        {
            var data = _playerService.GetPlayerDataByType(PlayerType.CommonHero);

            Player.Core.Player player = _playerService.CreatePlayerByPrefab(
                _playerInitData.CommonHero,
                _levelInitData.PlayerSpawnPosition);

            var playerCharacteristics = _playerService.InitPlayerCharacteristics(data);
            player.Construct(playerCharacteristics, _particleEffectsService);

            var playerTransform = player.transform;

            _cinemachineFreeLook.LookAt = playerTransform;
            _cinemachineFreeLook.Follow = playerTransform;

            PlayerIsSpawned?.Invoke();

            _playerService.SpawnPlayer();
            
            _player = player;
            _player.Health.LoadHealth(ObstacleSpawner.MaxMoney, ObstacleSpawner.CasualMoney);
            
            HealthBar = await ViewFactory.CreateHealthBar(player.Health);
            HealthBar.Show();

            _player.Health.HealthChanged += HandleHealthChanges;

            foreach (var turnTrigger in _turnTriggers)
            {
                turnTrigger.OnTurn += player.StartTurn;
            }
        }

        protected void GoToNextScene()
        {
            OnGoToNextScene?.Invoke();
        }

        private void HandleHealthChanges(float currentHealth, float maxHealth, float targetHealth)
        {
            if (targetHealth >= ObstacleSpawner.PoorMoney && targetHealth <= ObstacleSpawner.CasualMoney)
            {
                HealthBar.ChangePlayerCategory(Colors.GetColor(ColorName.CasualColor), "Обычный");
                _player.SetModel(PlayerModelType.Casual);
            }
            else if(targetHealth >= ObstacleSpawner.CasualMoney && targetHealth <= ObstacleSpawner.CoctailMoney)
            {
                HealthBar.ChangePlayerCategory(Colors.GetColor(ColorName.CoctailColor), "Получше");
                _player.SetModel(PlayerModelType.Coctail);
            }
            else if (targetHealth >= ObstacleSpawner.MiddleMoney && targetHealth <= ObstacleSpawner.BusinessMoney)
            {
                HealthBar.ChangePlayerCategory(Colors.GetColor(ColorName.MiddleColor), "Состоятельный");
                _player.SetModel(PlayerModelType.Middle);
            }
            else if (targetHealth >= ObstacleSpawner.BusinessMoney && targetHealth <= ObstacleSpawner.BlinqMoney)
            {
                HealthBar.ChangePlayerCategory(Colors.GetColor(ColorName.BusinessColor), "Бизнессмен");
                _player.SetModel(PlayerModelType.Business);
            }
            else if (targetHealth >= ObstacleSpawner.BlinqMoney)
            {
                HealthBar.ChangePlayerCategory(Colors.GetColor(ColorName.BlinqColor), "Богач");
                _player.SetModel(PlayerModelType.Blinq);
            }
            else if (targetHealth <= ObstacleSpawner.PoorMoney)
            {
                HealthBar.ChangePlayerCategory(Colors.GetColor(ColorName.PoorColor), "Бомж");
                _player.SetModel(PlayerModelType.Poor);
            }
        }

        private void InitSpawners(IObstacleService obstacleService)
        {
            ObstacleSpawner = new ObstacleSpawner(obstacleService, _audioSoundsService, _particleEffectsService);

            IsInitiatedSpawners?.Invoke();
        }
    }
}
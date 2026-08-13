using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DataBase.InitDataSO;
using Game.GameRoot;
using Player.Level.Spawners;
using Player.Level.Triggers;
using Reflex.Attributes;
using Services;
using UI.StateMachine;
using UI.View;
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
            await CreatePlayer();

            InitSpawners(_obstacleService);
        }

        protected async UniTask CreatePlayer()
        {
            var data = _playerService.GetPlayerDataByType(PlayerType.CommonHero);

            Player.Core.Player player = _playerService.CreatePlayerByPrefab(
                _playerInitData.CommonHero,
                _levelInitData.PlayerSpawnPosition);

            var playerCharacteristics = _playerService.InitPlayerCharacteristics(data);
            player.Construct(playerCharacteristics, _particleEffectsService);

            HealthBar = await ViewFactory.CreateHealthBar(player.Health);
            HealthBar.Show();

            var playerTransform = player.transform;

            _cinemachineFreeLook.LookAt = playerTransform;
            _cinemachineFreeLook.Follow = playerTransform;

            PlayerIsSpawned?.Invoke();

            _playerService.SpawnPlayer();
            
            _player = player;

            foreach (var turnTrigger in _turnTriggers)
            {
                turnTrigger.OnTurn += player.StartTurn;
            }
        }

        protected void CreateObstacles(int numberWave)
        {
        }

        protected void GoToNextScene()
        {
            OnGoToNextScene?.Invoke();
        }

        private void InitSpawners(IObstacleService obstacleService)
        {
            ObstacleSpawner = new ObstacleSpawner(obstacleService, _audioSoundsService, _particleEffectsService);

            IsInitiatedSpawners?.Invoke();
        }
    }
}
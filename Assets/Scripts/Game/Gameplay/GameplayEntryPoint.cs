using Audio.Sounds;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DataBase.InitDataSO;
using Game.Gameplay.Root.View;
using Game.GameRoot;
using Player.Level;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UI.Panel;
using UI.StateMachine.States;
using UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Game.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private const int MinCountValue = 0;
        private const int NextOperationStep = 1;

        [SerializeField] private CinemachineVirtualCamera _freeLookCamera;
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private DataFactory _dataFactory;
        [SerializeField] private ViewFactory _viewFactory;
        [SerializeField] private Mission _mission;
        
        private int _currentLevelIndex;
        
        private UIRootView _uiRoot;
        private UIGameplayRootBinder _uiScene;
        private Container _container;
        private GameplayExitParameters _exitParameters;

        private IObstacleService _obstacleService;
        private IDataBaseService _dataBaseService;
        private IPlayerService _playerService;
        private IFloatingTextService _floatingTextService;
        private ParticleEffectsService _particleEffectsService;
        private AudioSoundsService _audioSoundsService;
        private MissionService _missionService;
        private IUILocalizationService _uiLocalizationService;
        private IPauseService _pauseService;
        private ICurrencyService _currencyService;

        private ObstacleInitData _obstacleInitData;
        private PlayerInitData _playerInitData;
        
        private Level _currentLevel;
        private LevelInitData _currentLevelInitData;

        private EndGamePanel _endGamePanel;

        [Inject]
        private void Construct(
            IObstacleService obstacleService,
            IDataBaseService dataBaseService,
            IPlayerService playerService,
            ParticleEffectsService particleEffectsService,
            AudioSoundsService audioSoundsService,
            IFloatingTextService floatingTextService,
            MissionService missionService,
            IUILocalizationService uiLocalizationService,
            IPauseService pauseService,
            ICurrencyService currencyService)
        {
            _obstacleService = obstacleService;
            _dataBaseService = dataBaseService;
            _playerService = playerService;
            _particleEffectsService = particleEffectsService;
            _audioSoundsService = audioSoundsService;
            _floatingTextService = floatingTextService;
            _missionService = missionService;
            _uiLocalizationService = uiLocalizationService;
            _pauseService = pauseService;
            _currencyService = currencyService;
        }

        public async UniTask<Observable<GameplayExitParameters>> Run(
            UIRootView uiRoot,
            GameplayEnterParameters enterParameters = null)
        {
            _container = gameObject.scene.GetSceneContainer();
            _uiRoot = uiRoot;

            await _particleEffectsService.Init();

            _uiScene = Instantiate(_sceneUIRootPrefab);
            _viewFactory.GetEntities(uiRoot, _uiScene, _container, this);
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            GameObjectInjector.InjectRecursive(uiRoot.gameObject, _container);
            _uiScene.GetUIStateMachine(uiRoot.UIStateMachine, _uiRoot.UIRootButtons);

            await InitData();
            await _dataBaseService.Init();
            await _obstacleService.Init();
            await _playerService.Init();
            await _audioSoundsService.Init();
            await _missionService.Init();
            await _uiLocalizationService.Init();
            await _currencyService.Init();

            _audioSoundsService.PlayMusic(SoundsType.ActionMusic);
            _playerService.GetSceneObjects(_container, _freeLookCamera);

            // --- Создание первого уровня ---
            _currentLevelIndex = MinCountValue;
            await LoadLevel(_currentLevelIndex); // вынесли в отдельный метод

            var exitSceneSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSceneSignalSubject);

            uiRoot.UIStateMachine.EnterIn<GameplayState>();
            uiRoot.MoneyView.Show();
            OnShowJoystickWithAttackButton();

            var scene = SceneManager.GetActiveScene();
            if (scene.name == Scenes.Gameplay)
            {
                _endGamePanel = await _viewFactory.CreateEndGamePanel();
                _endGamePanel.GoToVillageButton.onClick.AddListener(_uiScene.HandleGoToNextScene);
                _endGamePanel.NextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
                uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _endGamePanel.SetLabelText;
            }
            else
            {
                YG2.SaveProgress();
                _currencyService.SaveMoney();
            }

            // Подписки на игрока уже сделаны в LoadLevel, поэтому здесь их не дублируем

            var exitToSceneSignal = exitSceneSignalSubject.Select(_ => _exitParameters);
            _uiScene.ResetCountdownTutorialPointer();
            return exitToSceneSignal;
        }

        private void OnDestroy()
        {
            var scene = SceneManager.GetActiveScene();

            if (_endGamePanel != null)
            {
                _endGamePanel.GoToVillageButton.onClick.RemoveListener(_uiScene.HandleGoToNextScene);
                _endGamePanel.NextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
                if (scene.name == Scenes.Gameplay)
                {
                    _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _endGamePanel.SetLabelText;
                }
            }

            // Отписываемся от текущего игрока
            if (_playerService != null && _playerService.Player != null)
            {
                UnsubscribeFromPlayerEvents(_playerService.Player);
            }

            if (_uiRoot != null)
            {
                _uiRoot.SettingsButton.onClick.RemoveListener(_playerService.Player.InputController.LockPlayerMovement);
                _uiRoot.LeaderboardButton.onClick.RemoveListener(_playerService.Player.InputController.LockPlayerMovement);
            }
        }

        public void GetGameplayExitParameters()
        {
            _audioSoundsService.PlayMusic(SoundsType.ActionMusic);
            _uiRoot.UIRootButtons.Deactivate();

            int nextNumberLevel = _missionService.CurrentNumberLevel + NextOperationStep;
            _missionService.SetCurrentNumberLevel(nextNumberLevel);

            var gameplayEnterParameters = new GameplayEnterParameters(Scenes.Gameplay, nextNumberLevel);
            _exitParameters = new GameplayExitParameters(gameplayEnterParameters);
        }

        private async UniTask LoadLevel(int index)
        {
            if (index < 0 || index >= _mission.Maps.Count)
                index = 0;
            
            if (_currentLevel != null)
            {
                if (_playerService.Player != null)
                    UnsubscribeFromPlayerEvents(_playerService.Player);
                _currentLevel.CleanupAndDestroy();
            }
            
            Level newLevel = Instantiate(_mission.Maps[index].Level);
            GameObjectInjector.InjectObject(newLevel.gameObject, _container);

            _currentLevel = newLevel;
            _currentLevelInitData = _mission.Maps[index];
            _currentLevelIndex = index;

            _obstacleService.GetData(_obstacleInitData);

            _currentLevel.GetDependencies(
                _currentLevelInitData,
                _playerInitData,
                _freeLookCamera,
                _viewFactory,
                _uiRoot.UIStateMachine,
                _uiRoot);

            FloatingTextView floatingTextView = await _viewFactory.CreateFloatingTextView();
            floatingTextView.Deactivate();
            _floatingTextService.Init(floatingTextView);

            await _currentLevel.OnStartLevel();
            
            if (_playerService.Player != null)
            {
                SubscribeToPlayerEvents(_playerService.Player);
            }
            
            if (_endGamePanel != null)
            {
                _playerService.Player.Health.Die += _endGamePanel.Show;
                _playerService.Player.Health.Die += _endGamePanel.SetDefeatPanel;
                _playerService.Player.Health.Die += _pauseService.OnStopGameWithoutMusic;
                _endGamePanel.OnSpawnPlayer += _currentLevel.HealthBar.Show;
                _endGamePanel.OnSpawnPlayer += _currentLevel.Respawn;
            }
        }

        private void SubscribeToPlayerEvents(Player.Core.Player player)
        {
            player.Health.Die += _uiScene.ResetCountdownTutorialPointer;
            player.InputController.OnMoveButtonsPressed += _uiScene.ResetCountdownTutorialPointer;
            _uiRoot.SettingsButton.onClick.AddListener(player.InputController.LockPlayerMovement);
            _uiRoot.LeaderboardButton.onClick.AddListener(player.InputController.LockPlayerMovement);
        }

        private void UnsubscribeFromPlayerEvents(Player.Core.Player player)
        {
            player.Health.Die -= _uiScene.ResetCountdownTutorialPointer;
            player.InputController.OnMoveButtonsPressed -= _uiScene.ResetCountdownTutorialPointer;
            _uiRoot.SettingsButton.onClick.RemoveListener(player.InputController.LockPlayerMovement);
            _uiRoot.LeaderboardButton.onClick.RemoveListener(player.InputController.LockPlayerMovement);
        }

        private void OnNextLevelButtonClick()
        {
            // _experiencePoints.ResetAccumulatedValues();
            _currencyService.ResetAccumulatedMoney();

            int nextIndex = (_currentLevelIndex + 1) % _mission.Maps.Count;
            _ = LoadLevel(nextIndex);

            _endGamePanel.Hide();
            _pauseService.OnPlayGame();
        }

        private async UniTask InitData()
        {
            _obstacleInitData = await _dataFactory.CreateSkeletonInitData();
            _playerInitData = await _dataFactory.CreatePlayerInitData();
        }

        private void OnShowJoystickWithAttackButton()
        {
            _playerService.GetJoystickWithAttackButton(_uiScene.Joystick);
            _uiScene.Joystick.gameObject.SetActive(!YG2.envir.isDesktop);
        }
    }
}
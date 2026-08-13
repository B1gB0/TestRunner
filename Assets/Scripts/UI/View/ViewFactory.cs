using Cysharp.Threading.Tasks;
using Game.Gameplay;
using Game.Gameplay.Root.View;
using Game.GameRoot;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using Services;
using UI.Panel;
using UnityEngine;

namespace UI.View
{
    public class ViewFactory : MonoBehaviour
    {
#if UNITY_EDITOR
        private const string CheatPanelPath = "CheatPanel";
#endif
        private const string HealthBarPath = "HealthBar";
        private const string TextViewPath = "TextView";
        private const string MoneyViewPath = "MoneyView";
        private const string ShopAttributePanelPath = "ShopAttributePanel";
        private const string EndGamePanelPath = "EndGamePanel";

        private IResourceService _resourceService;
        private IPlayerService _playerService;

        private UIRootView _uiRoot;
        private Container _container;
        
        private MissionChoosingPanel _missionChoosingPanel;
        private EndGamePanel _endGamePanel;
        
        public UIGameplayRootBinder UIScene { get; private set; }
        public GameplayEntryPoint GameplayEntryPoint { get; private set; }
        
        [Inject]
        public void Construct(IResourceService resourceService, IPlayerService playerService)
        {
            _resourceService = resourceService;
            _playerService = playerService;
        }

        private void OnDestroy()
        {
            // if (_objectiveTextView != null)
            //     _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _objectiveTextView.SetText;
            // if (_levelUpPanel != null)
            //     _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _levelUpPanel.OnLanguageChanged;
        }

        public void GetEntities(
            UIRootView uiRoot,
            UIGameplayRootBinder uiScene,
            Container container,
            GameplayEntryPoint gameplayEntryPoint)
        {
            _uiRoot = uiRoot;
            UIScene = uiScene;
            _container = container;
            
            GameplayEntryPoint = gameplayEntryPoint;

            GameObjectInjector.InjectRecursive(UIScene.gameObject, _container);
        }

        public async UniTask<HealthBar> CreateHealthBar(Health health)
        {
            var healthBarTemplate = await _resourceService.Load<GameObject>(HealthBarPath);
            healthBarTemplate = Instantiate(healthBarTemplate);

            HealthBar healthBar = healthBarTemplate.GetComponent<HealthBar>();
            GameObjectInjector.InjectSingle(healthBar.gameObject, _container);
            healthBar.Construct(health);
            healthBar.transform.SetParent(UIScene.transform, false);
            healthBar.GetPoints(UIScene.ShowHealthPoint, UIScene.HideHealthPoint, UIScene.WeaponPoint);

            return healthBar;
        }
        
        public async UniTask<FloatingTextView> CreateFloatingTextView()
        {
            var textViewTemplate = await _resourceService.Load<GameObject>(TextViewPath);
            textViewTemplate = Instantiate(textViewTemplate);
        
            FloatingTextView textView = textViewTemplate.GetComponent<FloatingTextView>();
            return textView;
        }
        
        public async UniTask<EndGamePanel> CreateEndGamePanel()
        {
            var endGamePanelTemplate = await _resourceService.Load<GameObject>(EndGamePanelPath);
            endGamePanelTemplate = Instantiate(endGamePanelTemplate);

            _endGamePanel = endGamePanelTemplate.GetComponent<EndGamePanel>();
            GameObjectInjector.InjectObject(_endGamePanel.gameObject, _container);
            _endGamePanel.transform.SetParent(UIScene.transform, false);
            _endGamePanel.gameObject.SetActive(false);

            return _endGamePanel;
        }
        
        public async UniTask<MoneyView> CreateMoneyView()
        {
            var moneyViewTemplate = await _resourceService.Load<GameObject>(MoneyViewPath);
            moneyViewTemplate = Instantiate(moneyViewTemplate);
        
            MoneyView moneyView = moneyViewTemplate.GetComponent<MoneyView>();
            GameObjectInjector.InjectObject(moneyView.gameObject, _container);
            moneyView.transform.SetParent(UIScene.transform);
            moneyView.GetPoints(UIScene.ShowMoneyPoint, UIScene.HideMoneyPoint);
        
            return moneyView;
        }
    }
}
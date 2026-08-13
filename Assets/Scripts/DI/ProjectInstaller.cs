using System.Collections.Generic;
using System.Linq;
using Game.GameRoot;
using Player.Experience;
using Reflex.Core;
using Reflex.Enums;
using Reflex.Injectors;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        private readonly List<object> _monoServices = new();
        private readonly List<GameObject> _monoServiceObjects = new();

        [SerializeField] private AudioSoundsService _audioSoundsServicePrefab;
        [SerializeField] private PlayerService _playerServicePrefab;
        [SerializeField] private ParticleEffectsService _particleEffectsService;
        [SerializeField] private UIRootView _uiRootViewPrefab;
        [SerializeField] private GameEntryPoint _gameEntryPointPrefab;
        [SerializeField] private MissionService _missionServicePrefab;
        
        private void OnDestroy()
        {
            foreach (var obj in _monoServiceObjects)
            {
                if (obj != null) Destroy(obj);
            }
        }

        public void InstallBindings(ContainerBuilder builder)
        {
            RegisterCoreServices(builder);
            CreateMonoServices();
            RegisterCreatedServices(builder);
            RegisterContainerDependentServices(builder);
        }

        private void RegisterCoreServices(ContainerBuilder builder)
        {
            RegisterTypeWithContracts<ResourceService, IResourceService>(builder);
            RegisterTypeWithContracts<DataBaseService, IDataBaseService>(builder);
            RegisterTypeWithContracts<ExperiencePoints, IExperiencePoints>(builder);
            RegisterTypeWithContracts<ObstacleService, IObstacleService>(builder);
            RegisterTypeWithContracts<PauseService, IPauseService>(builder);
            RegisterTypeWithContracts<FloatingTextService, IFloatingTextService>(builder);
            RegisterTypeWithContracts<CurrencyService, ICurrencyService>(builder);
            RegisterTypeWithContracts<TweenAnimationService, ITweenAnimationService>(builder);
            RegisterTypeWithContracts<UILocalizationService, IUILocalizationService>(builder);
        }
        
        private void RegisterTypeWithContracts<TImplementation, TContract>(ContainerBuilder builder)
        {
            builder.RegisterType(
                typeof(TImplementation),
                new[] { typeof(TContract) },
                Lifetime.Singleton, 
                Resolution.Lazy
            );
        }

        private void CreateMonoServices()
        {
            CreateService(_playerServicePrefab);
            CreateService(_audioSoundsServicePrefab);
            CreateService(_particleEffectsService);
            CreateService(_uiRootViewPrefab);
            CreateService(_gameEntryPointPrefab);
            CreateService(_missionServicePrefab);
        }

        private void CreateService<T>(T prefab)
            where T : MonoBehaviour
        {
            var instance = Instantiate(prefab);
            _monoServices.Add(instance);
            _monoServiceObjects.Add(instance.gameObject);
            DontDestroyOnLoad(instance);
        }

        private void RegisterCreatedServices(ContainerBuilder builder)
        {
            var method = typeof(ContainerBuilder).GetMethods()
                .FirstOrDefault(m => m.Name == "RegisterValue"
                                     && m.IsGenericMethodDefinition
                                     && m.GetParameters().Length == 1);

            foreach (var service in _monoServices)
            {
                var serviceType = service.GetType();
                
                var genericMethod = method.MakeGenericMethod(serviceType);
                genericMethod.Invoke(builder, new[] { service });
                
                foreach (var interfaceType in serviceType.GetInterfaces())
                {
                    genericMethod = method.MakeGenericMethod(interfaceType);
                    genericMethod.Invoke(builder, new[] { service });
                }
            }
        }

        private void RegisterContainerDependentServices(ContainerBuilder builder)
        {
            builder.OnContainerBuilt += container =>
            {
                foreach (var service in _monoServiceObjects)
                {
                    GameObjectInjector.InjectObject(service, container);
                }

                foreach (var service in _monoServices)
                {
                    if (service is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }
                }
            };
        }
    }
}
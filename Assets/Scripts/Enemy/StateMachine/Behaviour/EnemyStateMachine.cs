using System;
using System.Collections.Generic;
using Enemy.StateMachine.Behaviour.States;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine.Behaviour
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyStateMachine : MonoBehaviour
    {
        private Obstacle _obstacle;
        private NavMeshAgent _agent;

        private EnemyState _currentState;
        private Dictionary<Type, EnemyState> _states;

        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;

        private void Awake()
        {
            _obstacle = GetComponent<Obstacle>();
            _agent = GetComponent<NavMeshAgent>();
            
            _states = new Dictionary<Type, EnemyState>
            {
                { typeof(IdleState), new IdleState() },
                { typeof(DeathState), new DeathState() },
            };
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        public void InitializeAllStates()
        {
            foreach (var state in _states.Values)
            {
                state.Initialize(_obstacle, _agent, _particleEffectsService, _audioSoundsService);
            }
        }

        public void AddState(EnemyState state)
        {
            var type = state.GetType();

            if (_states.ContainsKey(type) == false)
            {
                _states.TryAdd(type, state);
            }
        }

        public void SwitchState<T>() where T : EnemyState
        {
            Type newStateType = typeof(T);
            if (!_states.ContainsKey(newStateType))
            {
                Debug.LogError($"State {newStateType} not found!");
                return;
            }

            _currentState?.Exit();
            _currentState = _states[newStateType];
            _currentState.Enter();
        }
        
        public void GetServices(AudioSoundsService audioSoundsService, ParticleEffectsService particleEffectsService)
        {
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
        }
    }
}
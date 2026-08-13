using DataBase.Data;
using Enemy.StateMachine.Animation;
using Services;
using UnityEngine.AI;

namespace Enemy.StateMachine.Behaviour.States
{
    public abstract class EnemyState
    {
        protected const int MinValue = 0;

        protected NavMeshAgent Agent;
        protected Obstacle Obstacle;
        protected ParticleEffectsService ParticleEffectsService;
        protected AudioSoundsService AudioSoundsService;
        
        protected EnemyAnimatedStateMachine AnimStateMachine => Obstacle.AnimatedStateMachine;
        protected Player.Core.Player Player => Obstacle.Player;
        protected ObstacleData Data => Obstacle.Data;

        public virtual void Initialize(
            Obstacle obstacle,
            NavMeshAgent agent,
            ParticleEffectsService particleEffectsService,
            AudioSoundsService audioSoundsService)
        {
            Obstacle = obstacle;
            Agent = agent;
            ParticleEffectsService = particleEffectsService;
            AudioSoundsService = audioSoundsService;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}
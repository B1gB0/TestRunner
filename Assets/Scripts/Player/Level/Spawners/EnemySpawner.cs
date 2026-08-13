using System;
using System.Collections.Generic;
using _Project.Scripts.Level.Spawners;
using Enemy.StateMachine.Behaviour.States;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Level.Spawners
{
    public class EnemySpawner
    {
        private const int MinValue = 0;
        private const int CorrectCountFactor = 1;
        private const float RandomPositionFactor = 1f;
        private const float OffsetYPolygonEnemies = 0.5f;
        private const float HatChance = 0.5f;

        private readonly IEnemyService _enemyService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ParticleEffectsService _particleEffectsService;

        private int _enemyCounter;
        private int _limitEnemies;

        public event Action OnPriestKilled;
        public event Action OnAllEnemiesKilled;

        public EnemySpawner(
            IEnemyService enemyService,
            int limitEnemies,
            AudioSoundsService audioSoundsService,
            ParticleEffectsService particleEffectsService)
        {
            _enemyService = enemyService;
            _limitEnemies = limitEnemies;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
        }

        public void SpawnWave(EnemyWave wave)
        {
            if (_enemyCounter > _limitEnemies - CorrectCountFactor)
                return;

            List<Vector3> spawnPoints = wave.WaveSpawnPoints;
            List<Vector3> patrolPoints = wave.PatrolPoints;

            if (spawnPoints == null || spawnPoints.Count == MinValue)
                return;

            List<Vector3> availableSpawnPoints = new List<Vector3>(spawnPoints);
        }
    }
}
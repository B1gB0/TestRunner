using System;
using System.Collections.Generic;
using Enemy.StateMachine.Behaviour.States;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Level.Spawners
{
    public class ObstacleSpawner
    {
        private const int MinValue = 0;
        private const int CorrectCountFactor = 1;
        private const float RandomPositionFactor = 1f;
        private const float OffsetYPolygonEnemies = 0.5f;
        private const float HatChance = 0.5f;

        private readonly IObstacleService _obstacleService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ParticleEffectsService _particleEffectsService;

        private int _enemyCounter;

        public ObstacleSpawner(
            IObstacleService obstacleService,
            AudioSoundsService audioSoundsService,
            ParticleEffectsService particleEffectsService)
        {
            _obstacleService = obstacleService;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
        }

        public void SpawnObstacles()
        {
            
        }

        public void SpawnWave(EnemyWave wave)
        {
            List<Vector3> spawnPoints = wave.WaveSpawnPoints;
            List<Vector3> patrolPoints = wave.PatrolPoints;

            if (spawnPoints == null || spawnPoints.Count == MinValue)
                return;

            List<Vector3> availableSpawnPoints = new List<Vector3>(spawnPoints);
        }
    }
}
using System;
using System.Collections.Generic;
using DataBase.InitDataSO;
using Enemy;
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

        public int MaxMoney { get; private set; }
        public int PoorMoney { get; private set; }
        public int MiddleMoney { get; private set; }
        public int BlinqMoney { get; private set; }
        public int CasualMoney { get; private set; }
        public int BusinessMoney { get; private set; }
        public int CoctailMoney { get; private set; }

        public ObstacleSpawner(
            IObstacleService obstacleService,
            AudioSoundsService audioSoundsService,
            ParticleEffectsService particleEffectsService)
        {
            _obstacleService = obstacleService;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
        }

        public void SpawnObstacles(LevelInitData levelInitData)
        {
            foreach (var spawnPoint in levelInitData.MoneySpawnPositions)
            {
                SpawnMoney(spawnPoint);
                MaxMoney += 2;
            }

            foreach (var spawnPoint in levelInitData.BottleSpawnPositions)
            {
                SpawnBottle(spawnPoint);
            }

            var stepMoney = MaxMoney / 6;
            PoorMoney = stepMoney;
            CasualMoney = 2 * stepMoney;
            CoctailMoney = 3 * stepMoney;
            MiddleMoney = 4 * stepMoney;
            BusinessMoney = 5 * stepMoney;
            BlinqMoney = 6 * stepMoney;
        }

        private Bottle SpawnBottle(Vector3 obstaclePosition)
        {
            Bottle bottle = _obstacleService.CreateBottle();

            var obstacleSpawnPosition = obstaclePosition;

            bottle.transform.position = obstacleSpawnPosition;

            return bottle;
        }

        private Money SpawnMoney(Vector3 obstaclePosition)
        {
            Money money = _obstacleService.CreateMoney();

            var obstacleSpawnPosition = obstaclePosition;

            money.transform.position = obstacleSpawnPosition;

            return money;
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
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

            // Проверка на null сервисов
            if (_obstacleService == null)
                Debug.LogError("[ObstacleSpawner] IObstacleService is null!");
            if (_audioSoundsService == null)
                Debug.LogWarning("[ObstacleSpawner] AudioSoundsService is null (not critical for spawn)");
            if (_particleEffectsService == null)
                Debug.LogWarning("[ObstacleSpawner] ParticleEffectsService is null (not critical for spawn)");
        }

        public void SpawnObstacles(LevelInitData levelInitData)
        {
            if (levelInitData == null)
            {
                Debug.LogError("[ObstacleSpawner] LevelInitData is null!");
                return;
            }

            Debug.Log($"[ObstacleSpawner] Starting SpawnObstacles. Money positions: {levelInitData.MoneySpawnPositions?.Count ?? 0}, Bottle positions: {levelInitData.BottleSpawnPositions?.Count ?? 0}");

            if (levelInitData.MoneySpawnPositions == null || levelInitData.MoneySpawnPositions.Count == 0)
                Debug.LogWarning("[ObstacleSpawner] MoneySpawnPositions list is null or empty!");

            if (levelInitData.BottleSpawnPositions == null || levelInitData.BottleSpawnPositions.Count == 0)
                Debug.LogWarning("[ObstacleSpawner] BottleSpawnPositions list is null or empty!");

            foreach (var spawnPoint in levelInitData.MoneySpawnPositions)
            {
                SpawnMoney(spawnPoint);
                MaxMoney += 2;
            }

            foreach (var spawnPoint in levelInitData.BottleSpawnPositions)
            {
                SpawnBottle(spawnPoint);
            }

            var stepMoney = MaxMoney / 7;
            PoorMoney = stepMoney;
            CasualMoney = 2 * stepMoney;
            CoctailMoney = 3 * stepMoney;
            MiddleMoney = 4 * stepMoney;
            BusinessMoney = 5 * stepMoney;
            BlinqMoney = 6 * stepMoney;

            Debug.Log($"[ObstacleSpawner] Spawned money = {MaxMoney / 2}, spawned bottles = {levelInitData.BottleSpawnPositions?.Count ?? 0}. MaxMoney = {MaxMoney}");
        }

        private Bottle SpawnBottle(Vector3 obstaclePosition)
        {
            Debug.Log($"[ObstacleSpawner] Trying to spawn Bottle at {obstaclePosition}");
            Bottle bottle = _obstacleService.CreateBottle();

            if (bottle == null)
            {
                Debug.LogError("[ObstacleSpawner] CreateBottle returned null!");
                return null;
            }

            bottle.transform.position = obstaclePosition;
            Debug.Log($"[ObstacleSpawner] Bottle spawned: {bottle.name}, active = {bottle.gameObject.activeSelf}, pos = {bottle.transform.position}");
            return bottle;
        }

        private Money SpawnMoney(Vector3 obstaclePosition)
        {
            Debug.Log($"[ObstacleSpawner] Trying to spawn Money at {obstaclePosition}");
            Money money = _obstacleService.CreateMoney();

            if (money == null)
            {
                Debug.LogError("[ObstacleSpawner] CreateMoney returned null!");
                return null;
            }

            money.transform.position = obstaclePosition;
            Debug.Log($"[ObstacleSpawner] Money spawned: {money.name}, active = {money.gameObject.activeSelf}, pos = {money.transform.position}");
            return money;
        }
    }
}
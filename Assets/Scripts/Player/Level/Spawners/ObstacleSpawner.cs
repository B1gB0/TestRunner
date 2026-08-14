using System.Collections.Generic;
using DataBase.InitDataSO;
using Enemy;
using Services;
using UnityEngine;

namespace Player.Level.Spawners
{
    public class ObstacleSpawner
    {
        private const int MinValue = 0;

        private readonly IObstacleService _obstacleService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ParticleEffectsService _particleEffectsService;

        private readonly List<Bottle> _spawnedBottles = new();
        private readonly List<Money> _spawnedMoney = new();

        private LevelInitData _currentLevelInitData;

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

        public void Respawn(LevelInitData levelInitData)
        {
            _currentLevelInitData = levelInitData;

            DeactivateAll();
            
            SpawnObstacles();
        }

        private void DeactivateAll()
        {
            foreach (var bottle in _spawnedBottles)
            {
                if (bottle != null)
                    bottle.gameObject.SetActive(false);
            }

            foreach (var money in _spawnedMoney)
            {
                if (money != null)
                    money.gameObject.SetActive(false);
            }

            _spawnedBottles.Clear();
            _spawnedMoney.Clear();
        }
        
        private void SpawnObstacles()
        {
            if (_currentLevelInitData == null) return;

            MaxMoney = 0;

            foreach (var spawnPoint in _currentLevelInitData.MoneySpawnPositions)
            {
                SpawnMoney(spawnPoint);
                MaxMoney += 2;
            }

            foreach (var spawnPoint in _currentLevelInitData.BottleSpawnPositions)
            {
                SpawnBottle(spawnPoint);
            }

            CalculateMoneyThresholds();
        }

        private Bottle SpawnBottle(Vector3 obstaclePosition)
        {
            Bottle bottle = _obstacleService.CreateBottle();
            if (bottle == null) return null;

            bottle.transform.position = obstaclePosition;
            _spawnedBottles.Add(bottle);
            return bottle;
        }

        private Money SpawnMoney(Vector3 obstaclePosition)
        {
            Money money = _obstacleService.CreateMoney();
            if (money == null) return null;

            money.transform.position = obstaclePosition;
            _spawnedMoney.Add(money);
            return money;
        }

        private void CalculateMoneyThresholds()
        {
            if (MaxMoney <= 0)
            {
                PoorMoney = CasualMoney = CoctailMoney = MiddleMoney = BusinessMoney = BlinqMoney = 0;
                return;
            }

            int stepMoney = MaxMoney / 7;
            PoorMoney = stepMoney;
            CasualMoney = 2 * stepMoney;
            CoctailMoney = 3 * stepMoney;
            MiddleMoney = 4 * stepMoney;
            BusinessMoney = 5 * stepMoney;
            BlinqMoney = 6 * stepMoney;
        }
    }
}
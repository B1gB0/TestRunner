using DataBase.Data;
using DataBase.InitDataSO;

namespace Services
{
    public interface IObstacleService : IService
    {
        public void GetData(EnemyInitData enemyInitData);
        // public EnemyData GetEnemyDataByType(EnemyType type);
    }
}
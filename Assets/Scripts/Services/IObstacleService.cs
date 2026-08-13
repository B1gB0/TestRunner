using DataBase.Data;
using DataBase.InitDataSO;

namespace Services
{
    public interface IObstacleService : IService
    {
        public void GetData(ObstacleInitData obstacleInitData);
        // public EnemyData GetEnemyDataByType(EnemyType type);
    }
}
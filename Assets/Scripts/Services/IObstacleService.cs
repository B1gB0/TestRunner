using DataBase.Data;
using DataBase.InitDataSO;
using Enemy;

namespace Services
{
    public interface IObstacleService : IService
    {
        public void GetData(ObstacleInitData obstacleInitData);
        public Money CreateMoney();
        public Bottle CreateBottle();
        // public EnemyData GetEnemyDataByType(EnemyType type);
    }
}
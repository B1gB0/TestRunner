using DataBase.Data;
using DataBase.InitDataSO;

namespace Services
{
    public interface IEnemyService : IService
    {
        public void GetData(EnemyInitData enemyInitData);
        // public EnemyData GetEnemyDataByType(EnemyType type);
    }
}
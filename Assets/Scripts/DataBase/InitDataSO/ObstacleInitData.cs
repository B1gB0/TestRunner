using Enemy;
using UnityEngine;

namespace DataBase.InitDataSO
{
    [CreateAssetMenu(menuName = "InitData/EnemyInitData")]
    public class ObstacleInitData : InitData
    {
        [field: SerializeField] public Bottle BottlePrefab { get; private set; }
        [field: SerializeField] public Money MoneyPrefab { get; private set; }
    }
}
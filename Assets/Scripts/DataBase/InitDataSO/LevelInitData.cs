using System.Collections.Generic;
using UnityEngine;

namespace DataBase.InitDataSO
{
    [CreateAssetMenu(menuName = "InitData/LevelData")]
    public class LevelInitData : InitData
    {
        public List<Vector3> BottleSpawnPositions;

        public List<Vector3> MoneySpawnPositions;

        public Vector3 PlayerSpawnPosition;
    }
}
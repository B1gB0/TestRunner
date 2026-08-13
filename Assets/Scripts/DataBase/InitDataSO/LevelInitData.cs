using System.Collections.Generic;
using Player.Level;
using UnityEngine;

namespace DataBase.InitDataSO
{
    [CreateAssetMenu(menuName = "InitData/LevelData")]
    public class LevelInitData : InitData
    {
        public Level Level;
        
        public List<Vector3> BottleSpawnPositions;

        public List<Vector3> MoneySpawnPositions;

        public Vector3 PlayerSpawnPosition;
    }
}
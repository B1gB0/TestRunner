using System;
using Enemy;
using UnityEngine;

namespace DataBase.Data
{
    [Serializable]
    public class ObstacleData
    {
        [SerializeField] private ObstacleType _type;
        [SerializeField] private int _money;
        
        public ObstacleType Type => _type;
        public int Money => _money;
    }
}
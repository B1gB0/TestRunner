using System;
using Enemy;
using UnityEngine;

namespace DataBase.Data
{
    [Serializable]
    public class ObstacleData
    {
        [SerializeField] private ObstacleType _type;
        [SerializeField] private float _health;
        [SerializeField] private float _damage;
        [SerializeField] private int _money;
        
        public ObstacleType Type => _type;
        public float Health => _health;
        public float Damage => _damage;
        public int Money => _money;
    }
}
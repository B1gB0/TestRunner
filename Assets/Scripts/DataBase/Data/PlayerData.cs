using System;
using Player;
using UnityEngine;

namespace DataBase.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private PlayerType _type;
        [SerializeField] private float _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        public PlayerType Type => _type;
        public float Health => _health;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
    }
}
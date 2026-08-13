using System;
using UnityEngine;

namespace DataBase.Data
{
    [Serializable]
    public class PlayerLevelData
    {
        [SerializeField] private int _requiredExperience;
        
        public int RequiredExperience => _requiredExperience;
    }
}
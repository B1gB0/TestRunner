using System.Collections.Generic;
using UnityEngine;

namespace Level_Manager
{
    [CreateAssetMenu(menuName = "Data/Lvls List")]
    public class LevelsList : ScriptableObject
    {
        public bool randomizedLvls;
        public List<Level> lvls;
    }
}
using UnityEngine;

namespace DataBase.InitDataSO
{
    [CreateAssetMenu(menuName = "InitData/PlayerInitData")]
    public class PlayerInitData : InitData
    {
        [field: SerializeField] public Player.Core.Player CommonHero { get; private set; }
    }
}
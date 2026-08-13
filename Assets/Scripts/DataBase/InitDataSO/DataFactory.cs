using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Services;
using UnityEngine;

namespace DataBase.InitDataSO
{
    public class DataFactory : MonoBehaviour
    {
        private const string PlayerInitData = "PlayerInitData";
        private const string EnemyInitData = "EnemyInitData";

        private IResourceService _resourceService;

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public async UniTask<ObstacleInitData> CreateSkeletonInitData()
        {
            var skeletonData = await _resourceService.Load<ObstacleInitData>(EnemyInitData);
            return skeletonData;
        }
        
        public async UniTask<PlayerInitData> CreatePlayerInitData()
        {
            var playerData = await _resourceService.Load<PlayerInitData>(PlayerInitData);
            return playerData;
        }
    }
}
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;

namespace Player.Level
{
    public class FirstLevel : Level
    {
        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        public override async UniTask OnStartLevel()
        {
            await base.OnStartLevel();
        }
        
        private void HandleMissionTransition()
        {
            ViewFactory.GameplayEntryPoint.GetGameplayExitParameters();
            ViewFactory.UIScene.HandleGoToNextScene();
        }
    }
}
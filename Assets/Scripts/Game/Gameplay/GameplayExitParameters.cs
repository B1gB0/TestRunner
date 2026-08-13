using Game.GameRoot;

namespace Game.Gameplay
{
    public class GameplayExitParameters
    {
        public readonly SceneEnterParameters TargetSceneEnterParameters;

        public GameplayExitParameters(SceneEnterParameters targetSceneEnterParameters)
        {
            TargetSceneEnterParameters = targetSceneEnterParameters;
        }
    }
}
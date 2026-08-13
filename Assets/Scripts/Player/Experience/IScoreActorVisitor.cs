using UI.Panel;

namespace Player.Experience
{
    public interface IScoreActorVisitor
    {
        public void Visit(IExperienceScoreActor experienceScoreActor);
#if UNITY_EDITOR
        public void Visit(CheatPanel cheatPanel);
#endif
    }
}
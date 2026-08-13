namespace Player.Experience
{
    public interface IExperienceScoreActor
    {
        public int Experience { get; }
        public int Score { get; }
        public bool IsEnemy { get; }
    }
}
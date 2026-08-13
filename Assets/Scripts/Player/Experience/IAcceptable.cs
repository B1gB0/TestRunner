namespace Player.Experience
{
    public interface IAcceptable
    {
        public void AcceptScore(IScoreActorVisitor visitor) { }
    }
}

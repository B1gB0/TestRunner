namespace UI.StateMachine
{
    public abstract class UIState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}
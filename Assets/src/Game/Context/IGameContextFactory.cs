namespace Curry.Game
{
    public interface IGameContext
    {
    }

    public delegate void OnContextUpdate<T>(T context) where T : IGameContext;
    public interface IGameContextFactory <T> where T : IGameContext
    {
        public event OnContextUpdate<T> OnUpdate;

        public void UpdateContext(T context);

        public T Context { get; }
    }
}

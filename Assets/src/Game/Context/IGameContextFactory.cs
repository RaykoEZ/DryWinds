namespace Curry.Game
{
    public interface IGameContext
    {
    }

    public interface IGameContextFactory <T> where T : IGameContext
    {
        public void UpdateContext(T context);

        public T Context();
    }
}
